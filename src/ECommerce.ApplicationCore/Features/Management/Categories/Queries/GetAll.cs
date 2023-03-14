using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Management.Categories.Queries;

public class GetAllCategoriesQuery : IQuery<List<ManagementCategoryReadModel>> { }

public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, List<ManagementCategoryReadModel>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllCategoriesQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<List<ManagementCategoryReadModel>> HandleAsync(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Categories
            .Select(x => ManagementCategoryReadModel.FromCategory(x))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        return result;
    }
}
