using ECommerce.ApplicationCore.Features.Customers.Products.Exceptions;
using ECommerce.ApplicationCore.Features.Customers.Products.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Customers.Products.Queries;

public class GetCustomerProductByIdQuery : IQuery<CustomerProductReadModel>
{
    public required long Id { get; init; }
}

public class GetProductByIdQueryHandler : IQueryHandler<GetCustomerProductByIdQuery, CustomerProductReadModel>
{
    private readonly IAppDbContext _dbContext;

    public GetProductByIdQueryHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async ValueTask<CustomerProductReadModel> HandleAsync(GetCustomerProductByIdQuery query, CancellationToken cancellationToken)
    {
        var readModel = await _dbContext.Products
            .Select(x => CustomerProductReadModel.From(x))
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (readModel is null)
            throw new CustomerProductNotFoundByIdException(query.Id);

        return readModel;
    }
}