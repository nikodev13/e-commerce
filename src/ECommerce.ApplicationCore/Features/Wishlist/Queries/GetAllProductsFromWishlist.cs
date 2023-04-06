using ECommerce.ApplicationCore.Features.CustomerAccounts.ReadModels;
using ECommerce.ApplicationCore.Features.Wishlist.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Wishlist.Queries;

public record GetAllProductsFromWishlistQuery : IQuery<List<WishlistProductReadModel>>;

public class GetAllProductsFromWishlistQueryHandler : IQueryHandler<GetAllProductsFromWishlistQuery, List<WishlistProductReadModel>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public GetAllProductsFromWishlistQueryHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask<List<WishlistProductReadModel>> HandleAsync(GetAllProductsFromWishlistQuery query, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;
        var wishlist = await _dbContext.WishlistProducts
            .Include(x => x.Product)
            .Where(x => x.CustomerId == customerId)
            .Select(x => WishlistProductReadModel.From(x))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return wishlist;
    }
}