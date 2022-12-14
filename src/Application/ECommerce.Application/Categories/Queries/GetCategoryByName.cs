using ECommerce.Application.Categories.ReadModels;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Categories.Queries;

public class GetCategoryByNameQuery : IQuery<CategoryReadModel>
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
            .NotEmpty()
            .MinimumLength(3);
    }
}

public class GetCategoryByNameQueryHandler : IQueryHandler<GetCategoryByNameQuery, CategoryReadModel>
{
    private readonly IApplicationDatabase _database;

    public GetCategoryByNameQueryHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<CategoryReadModel> Handle(GetCategoryByNameQuery request, CancellationToken cancellationToken)
    {
        var category = await _database.Categories.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == request.CategoryName, cancellationToken);
        
        if (category is null) 
            throw new NotFoundException($"Category with name {request.CategoryName} not found.");

        var result = CategoryReadModel.FromCategory(category);
        return result;
    }
}

