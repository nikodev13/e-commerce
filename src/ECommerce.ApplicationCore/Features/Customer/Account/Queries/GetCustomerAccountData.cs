using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Customer.Account.Queries;

public class GetCustomerAccountQuery : IQuery<CustomerAccountReadModel> { }

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
        var account = await _dbContext.CustomersAccounts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == customerId, cancellationToken);

        return CustomerAccountReadModel.FromCustomerAccount(account!);
    }
}