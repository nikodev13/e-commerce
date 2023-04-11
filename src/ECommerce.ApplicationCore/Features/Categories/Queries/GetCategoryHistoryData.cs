using ECommerce.ApplicationCore.Features.Categories.Exceptions;
using ECommerce.ApplicationCore.Features.Categories.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Categories.Queries;

public record GetCategoryHistoryDataQuery(long Id) : IQuery<CategoryHistoryReadModel>;

internal sealed class GetCategoryHistoryDataQueryHandler : IQueryHandler<GetCategoryHistoryDataQuery, CategoryHistoryReadModel>
{
    private readonly IAppDbContext _dbContext;

    public GetCategoryHistoryDataQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<CategoryHistoryReadModel> HandleAsync(GetCategoryHistoryDataQuery query, CancellationToken cancellationToken)
    {
        var categoryHistoryReadModel = await _dbContext.Categories
            .Where(x => x.Id == query.Id)
            .Select(x => new CategoryHistoryReadModel(x.Id, x.LastModifiedBy, x.LastModifiedAt, x.CreatedBy, x.CreatedAt))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return categoryHistoryReadModel ?? throw new CategoryNotFoundException(query.Id);
    }
}