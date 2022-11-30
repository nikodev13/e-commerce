using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.ProductCategories.Features.Queries;

public class GetCategoryByNameQuery : IRequest<CategoryDto>
{
    public GetCategoryByNameQuery(string categoryName)
    {
        CategoryName = categoryName;
    }
    public string CategoryName { get; }
}

public class GetCategoryByNameQueryHandler : IRequestHandler<GetCategoryByNameQuery, CategoryDto>
{
    private readonly IApplicationDatabase _database;

    public GetCategoryByNameQueryHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<CategoryDto> Handle(GetCategoryByNameQuery request, CancellationToken cancellationToken)
    {
        var category = await _database.Categories.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == request.CategoryName, cancellationToken);
        
        if (category is null) 
            throw new NotFoundException($"Category with name {request.CategoryName} not found.");

        var result = CategoryDto.FromCategory(category);
        return result;
    }
}

