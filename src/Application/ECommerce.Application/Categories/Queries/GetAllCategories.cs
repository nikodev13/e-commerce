using ECommerce.Application.Categories.Models;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Categories.Queries;

public class GetAllCategoriesQuery : IQuery<List<CategoryDto>>
{
}

public class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly IApplicationDatabase _database;

    public GetAllCategoriesQueryHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<Result<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _database.Categories.AsNoTracking()
            .Select(x => CategoryDto.FromCategory(x)).ToListAsync(cancellationToken);
        return result;
    }
}
