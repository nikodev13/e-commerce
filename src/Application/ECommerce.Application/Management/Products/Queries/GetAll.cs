using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Management.Products.Queries;

public class GetAllProductsQuery : IQuery<List<ProductReadModel>> { }

public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, List<ProductReadModel>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllProductsQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<List<ProductReadModel>> HandleAsync(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .Select(p => ProductReadModel.FromProduct(p))
            .ToListAsync(cancellationToken);

        return result;
    }
}
