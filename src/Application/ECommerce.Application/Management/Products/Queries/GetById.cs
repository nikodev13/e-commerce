using ECommerce.Application.Management.Products.Exceptions;
using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Management.Products.Queries;

public class GetProductByIdQuery : IQuery<ProductReadModel>
{
    public required long Id { get; init; }
}

public class GetProductByIdQueryValidator : AbstractValidator<GetProductByIdQuery>
{
    public GetProductByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

public class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductReadModel>
{
    private readonly IAppDbContext _dbContext;

    public GetProductByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ProductReadModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
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