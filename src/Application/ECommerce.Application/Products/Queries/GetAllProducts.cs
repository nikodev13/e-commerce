using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Products.ReadModels;
using ECommerce.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Products.Queries;

public class GetAllProductsQuery : IQuery<List<ProductReadModel>>
{
    
}

public class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, List<ProductReadModel>>
{
    private readonly IApplicationDatabase _database;

    public GetAllProductsQueryHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<Result<List<ProductReadModel>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await _database.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .Select(p => ProductReadModel.FromProduct(p))
            .ToListAsync(cancellationToken);

        return result;
    }
}
