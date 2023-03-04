using ECommerce.ApplicationCore.Features.Management.Categories.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Management.Categories.Queries;

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