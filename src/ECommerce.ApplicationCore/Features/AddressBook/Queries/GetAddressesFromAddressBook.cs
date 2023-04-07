using ECommerce.ApplicationCore.Features.AddressBook.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.AddressBook.Queries;

public record GetAddressesFromAddressBookQuery : IQuery<List<AddressReadModel>>;

internal sealed class GetAddressesFromAddressBookQueryHandler : IQueryHandler<GetAddressesFromAddressBookQuery, List<AddressReadModel>>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public GetAddressesFromAddressBookQueryHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask<List<AddressReadModel>> HandleAsync(GetAddressesFromAddressBookQuery query, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;
        var addressBook = await _dbContext.Addresses
            .Where(x => x.CustomerId == customerId)
            .Select(x => AddressReadModel.From(x))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return addressBook;
    }
}
