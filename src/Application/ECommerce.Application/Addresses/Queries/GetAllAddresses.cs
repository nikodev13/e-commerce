using ECommerce.Application.Addresses.ReadModels;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Addresses.Queries;

public class GetAllAddressesQuery : IQuery<List<AddressReadModel>>
{
}

public class GetAllAddressesQueryHandler : IQueryHandler<GetAllAddressesQuery, List<AddressReadModel>>
{
    private readonly IApplicationDatabase _database;
    private readonly IUserContextService _userContextService;

    public GetAllAddressesQueryHandler(IApplicationDatabase database, IUserContextService userContextService)
    {
        _database = database;
        _userContextService = userContextService;
    }
    
    public async Task<List<AddressReadModel>> Handle(GetAllAddressesQuery request, CancellationToken cancellationToken)
    {
        var result = await _database.Addresses.Select(x => AddressReadModel.FromAddress(x)).ToListAsync(cancellationToken);
        return result;
    }
}