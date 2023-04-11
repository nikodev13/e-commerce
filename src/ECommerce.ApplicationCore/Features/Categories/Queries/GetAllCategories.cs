using ECommerce.ApplicationCore.Features.Categories.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Categories.Queries;

public class GetAllCategoriesQuery : IQuery<List<CategoryReadModel>> { }

internal sealed class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, List<CategoryReadModel>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllCategoriesQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<List<CategoryReadModel>> HandleAsync(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        var readModels = await _dbContext.Categories
            .Select(x => new CategoryReadModel(x.Id, x.Name))
            .ToListAsync(cancellationToken);

        return readModels;
    }
}