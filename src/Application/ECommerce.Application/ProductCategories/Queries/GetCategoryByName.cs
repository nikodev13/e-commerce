using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Results.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.ProductCategories.Queries;

public class GetCategoryByNameQuery : IQuery<CategoryDto>
{
    public GetCategoryByNameQuery(string categoryName)
    {
        CategoryName = categoryName;
    }
    public string CategoryName { get; }
}

public class GetCategoryByNameQueryValidator : AbstractValidator<GetCategoryByNameQuery>
{
    public GetCategoryByNameQueryValidator()
    {
        RuleFor(x => x.CategoryName)
            .MinimumLength(3);
    }
}

public class GetCategoryByNameQueryHandler : IQueryHandler<GetCategoryByNameQuery, CategoryDto>
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

