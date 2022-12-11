using ECommerce.Application.Categories.Models;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Results.Errors;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Categories.Queries;

public class GetCategoryByIdQuery : IQuery<CategoryDto>
{
    public GetCategoryByIdQuery(long categoryId)
    {
        CategoryId = categoryId;
    }
    
    public long CategoryId { get; }
}

public class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty();
    }
}

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly IApplicationDatabase _database;

    public GetCategoryByIdQueryHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _database.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (category is null) 
            return new NotFoundError($"Category with ID {request.CategoryId} not found.");
            
        var result = CategoryDto.FromCategory(category);
        return result;
    }
}