using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.ProductCategories.Queries;

public class GetAllCategoriesQuery : IRequest<Result<List<CategoryDto>>> 
{
}

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Result<List<CategoryDto>>>
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
