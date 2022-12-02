using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Results.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.ProductCategories.Queries;

public class GetCategoryByNameQuery : IRequest<Result<CategoryDto>>
{
    public GetCategoryByNameQuery(string categoryName)
    {
        CategoryName = categoryName;
    }
    public string CategoryName { get; }
}

public class GetCategoryByNameQueryHandler : IRequestHandler<GetCategoryByNameQuery, Result<CategoryDto>>
{
    private readonly IApplicationDatabase _database;

    public GetCategoryByNameQueryHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<Result<CategoryDto>> Handle(GetCategoryByNameQuery request, CancellationToken cancellationToken)
    {
        var category = await _database.Categories.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == request.CategoryName, cancellationToken);
        
        if (category is null) 
            return new NotFoundError($"Category with name {request.CategoryName} not found.");

        var result = CategoryDto.FromCategory(category);
        return result;
    }
}

