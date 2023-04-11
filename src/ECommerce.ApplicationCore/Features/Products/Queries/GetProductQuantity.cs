using ECommerce.ApplicationCore.Features.Products.Exceptions;
using ECommerce.ApplicationCore.Features.Products.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Products.Queries;

public record GetProductQuantityQuery(long Id) : IQuery<ProductQuantityReadModel>;

internal sealed class GetProductQuantityQueryHandler : IQueryHandler<GetProductQuantityQuery, ProductQuantityReadModel>
{
    private readonly IAppDbContext _dbContext;

    public GetProductQuantityQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<ProductQuantityReadModel> HandleAsync(GetProductQuantityQuery query, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .Where(x => x.Id == query.Id)
            .Select(x => new ProductQuantityReadModel(x.Id, x.InStockQuantity))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        
        return product ?? throw new ProductNotFoundByIdException(query.Id);
    }
}