using ECommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.ProductCategories.Features.Queries;

public class GetAllCategoriesQuery : IRequest<List<CategoryDto>> 
{
}

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly IApplicationDatabase _database;

    public GetAllCategoriesQueryHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _database.Categories.AsNoTracking()
            .Select(x => CategoryDto.FromCategory(x)).ToListAsync(cancellationToken);
        return result;
    }
}
