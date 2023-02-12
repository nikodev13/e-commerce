using ECommerce.Application.Management.Categories.Exceptions;
using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Management.Categories.Queries;

public class GetCategoryByIdQuery : IQuery<CategoryReadModel>
{
    public required long Id { get; init; }
}

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryReadModel>
{
    private readonly IAppDbContext _dbContext;

    public GetCategoryByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<CategoryReadModel> HandleAsync(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
        if (category is null) 
            throw new CategoryNotFoundException(request.Id);
        var result = CategoryReadModel.FromCategory(category);
        return result;
    }
}

public class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}