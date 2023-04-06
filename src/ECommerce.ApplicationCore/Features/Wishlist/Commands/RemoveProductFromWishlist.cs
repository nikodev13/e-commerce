using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Wishlist.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Wishlist.Commands;

public record RemoveProductFromWishlistCommand(long ProductId) : ICommand;

public class RemoveProductFromWishlistCommandHandler : ICommandHandler<RemoveProductFromWishlistCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public RemoveProductFromWishlistCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask HandleAsync(RemoveProductFromWishlistCommand command, CancellationToken cancellationToken)
    {
        var productId = command.ProductId;
        var customerId = _userContextProvider.UserId!.Value;

        var wishlistItem = await _dbContext.WishlistProducts
                .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.ProductId == productId, cancellationToken);

        if (wishlistItem is null) throw new ProductNotFoundInWishlistException(productId);

        _dbContext.WishlistProducts.Remove(wishlistItem);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
