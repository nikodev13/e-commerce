using ECommerce.ApplicationCore.Features.Management.Products.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Management.Products.Queries;

public class GetProductByIdQuery : IQuery<ProductReadModel>
{
    public required long Id { get; init; }
}

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductReadModel>
{
    private readonly IAppDbContext _dbContext;

    public GetProductByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<ProductReadModel> HandleAsync(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .Include(x => x.Category)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(request.Id);

        var result = ProductReadModel.FromProduct(product);
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
