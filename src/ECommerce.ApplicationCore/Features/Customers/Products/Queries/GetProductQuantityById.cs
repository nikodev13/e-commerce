using ECommerce.ApplicationCore.Features.Customers.Products.Exceptions;
using ECommerce.ApplicationCore.Features.Customers.Products.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Customers.Products.Queries;

public class GetProductQuantityByIdQuery : IQuery<CustomerProductQuantityReadModel>
{
    public required long Id { get; init; }
}

public class GetProductQuantityByIdQueryHandler : IQueryHandler<GetProductQuantityByIdQuery, CustomerProductQuantityReadModel>
{
    private readonly IAppDbContext _dbContext;

    public GetProductQuantityByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<CustomerProductQuantityReadModel> HandleAsync(GetProductQuantityByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (product is null)
            throw new CustomerProductNotFoundByIdException(query.Id);

        return new CustomerProductQuantityReadModel
        {
            ProductId = product.Id,
            InStockQuantity = product.InStockQuantity
        };
    }
}