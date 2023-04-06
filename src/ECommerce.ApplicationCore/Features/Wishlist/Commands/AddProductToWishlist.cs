using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Products.Exceptions;
using ECommerce.ApplicationCore.Features.Wishlist.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Wishlist.Commands;

public record AddProductToWishlistCommand(long ProductId) : ICommand;

public class AddProductToWishlistCommandHandler : ICommandHandler<AddProductToWishlistCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public AddProductToWishlistCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask HandleAsync(AddProductToWishlistCommand command, CancellationToken cancellationToken)
    {
        var productId = command.ProductId;
        var customerId = _userContextProvider.UserId!.Value;

        var productsInWishlist = await _dbContext.WishlistProducts
            .Where(x => x.CustomerId == customerId)
            .ToListAsync(cancellationToken);
        
        var alreadyExistsInWishList = productsInWishlist.Any(x => x.ProductId == productId);
        if (alreadyExistsInWishList) throw new ProductAlreadyExistsInWishlistException(command.ProductId);

        if (productsInWishlist.Count >= 10) throw new WishlistHasMaxNumberOfProductsException();
        
        var productExists = await _dbContext.Products
            .AnyAsync(x => x.Id == productId, cancellationToken);
        if (productExists!) throw new ProductNotFoundException(productId);

        var wishlistProduct = new WishlistProduct
        {
            CustomerId = customerId,
            ProductId = productId
        };

        await _dbContext.WishlistProducts.AddAsync(wishlistProduct, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
