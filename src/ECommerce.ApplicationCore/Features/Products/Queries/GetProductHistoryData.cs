using ECommerce.ApplicationCore.Features.Products.Exceptions;
using ECommerce.ApplicationCore.Features.Products.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Products.Queries;

public record GetProductHistoryDataQuery(long Id) : IQuery<ProductHistoryReadModel>;

internal sealed class GetProductHistoryDataQueryHandler : IQueryHandler<GetProductHistoryDataQuery, ProductHistoryReadModel>
{
    private readonly IAppDbContext _dbContext;

    public GetProductHistoryDataQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<ProductHistoryReadModel> HandleAsync(GetProductHistoryDataQuery query, CancellationToken cancellationToken)
    {
        var productHistoryReadModel = await _dbContext.Products
            .Where(x => x.Id == query.Id)
            .Select(x =>
                new ProductHistoryReadModel(x.Id, x.LastModifiedBy, x.LastModifiedAt, x.CreatedBy, x.CreatedAt))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return productHistoryReadModel ?? throw new ProductNotFoundException(query.Id);
    }
}