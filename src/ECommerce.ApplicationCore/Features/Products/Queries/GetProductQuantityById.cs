using ECommerce.ApplicationCore.Features.Products.Exceptions;
using ECommerce.ApplicationCore.Features.Products.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Products.Queries;

public record GetProductQuantityByIdQuery(long Id) : IQuery<ProductQuantityReadModel>;

public class GetProductQuantityByIdQueryHandler : IQueryHandler<GetProductQuantityByIdQuery, ProductQuantityReadModel>
{
    private readonly IAppDbContext _dbContext;

    public GetProductQuantityByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<ProductQuantityReadModel> HandleAsync(GetProductQuantityByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .Select(x => new ProductQuantityReadModel(x.Id, x.InStockQuantity))
            .FirstOrDefaultAsync(x => x.ProductId == query.Id, cancellationToken);
        
        return product ?? throw new CustomerProductNotFoundByIdException(query.Id);
    }
}