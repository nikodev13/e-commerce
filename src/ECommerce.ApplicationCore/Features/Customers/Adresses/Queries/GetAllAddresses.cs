using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Customers.Adresses.Queries;

public class GetAllAddressesCommand : IQuery<List<AddressReadModel>> { }

public class GetAllAddressesCommandHandler : IQueryHandler<GetAllAddressesCommand, List<AddressReadModel>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public GetAllAddressesCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask<List<AddressReadModel>> HandleAsync(GetAllAddressesCommand query, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;
        var result = await _dbContext.Addresses.Where(x => x.CustomerId == customerId)
            .Select(x => AddressReadModel.From(x))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return result;
    }
}
