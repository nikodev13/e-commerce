using ECommerce.ApplicationCore.Features.Customers.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Customers.Queries;

public class GetCustomerAccountQuery : IQuery<CustomerAccountReadModel>
{
    public static GetCustomerAccountQuery Instance { get; } = new();
    private GetCustomerAccountQuery() { }
}

public class GetCustomerAccountQueryHandler : IQueryHandler<GetCustomerAccountQuery, CustomerAccountReadModel>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public GetCustomerAccountQueryHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask<CustomerAccountReadModel> HandleAsync(GetCustomerAccountQuery query, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;
        var account = await _dbContext.Customers
            .Where(x => x.Id == customerId)
            .Select(x => CustomerAccountReadModel.From(x))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return account!;
    }
}