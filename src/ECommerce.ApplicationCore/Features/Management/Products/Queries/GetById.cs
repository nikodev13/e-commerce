using ECommerce.ApplicationCore.Features.Management.Products.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Management.Products.Queries;

public class GetProductByIdQuery : IQuery<ManagementProductReadModel>
{
    public required long Id { get; init; }
}

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ManagementProductReadModel>
{
    private readonly IAppDbContext _dbContext;

    public GetProductByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<ManagementProductReadModel> HandleAsync(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(query.Id);

        var result = ManagementProductReadModel.From(product);
        return result;
    }
}

public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
