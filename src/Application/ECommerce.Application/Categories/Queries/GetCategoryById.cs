using ECommerce.Application.Categories.ReadModels;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Categories.Queries;

public class GetCategoryByIdQuery : IQuery<CategoryReadModel>
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

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryReadModel>
{
    private readonly IApplicationDatabase _database;

    public GetCategoryByIdQueryHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<CategoryReadModel> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _database.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (category is null) 
            throw new NotFoundException($"Category with ID {request.CategoryId} not found.");
            
        var result = CategoryReadModel.FromCategory(category);
        return result;
    }
}