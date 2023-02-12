using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Management.Categories.Queries;

public class GetAllCategoriesQuery : IQuery<List<CategoryReadModel>> { }

public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, List<CategoryReadModel>>
{
    private readonly IAppDbContext _dbContext;

    public GetAllCategoriesQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<List<CategoryReadModel>> HandleAsync(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Categories
            .Select(x => CategoryReadModel.FromCategory(x))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        return result;
    }
}
