using ECommerce.ApplicationCore.Features.CustomerAccounts.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.CustomerAccounts.Commands;

public record DeleteCustomerAddressCommand(long Id) : ICommand;

public class DeleteCustomerAddressCommandHandler : ICommandHandler<DeleteCustomerAddressCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public DeleteCustomerAddressCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask HandleAsync(DeleteCustomerAddressCommand command, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;
        var address = await _dbContext.Addresses.FirstOrDefaultAsync(x => x.Id == command.Id && x.CustomerId == customerId, cancellationToken);
        if (address is null)
            throw new AddressNotFoundException(command.Id);

        _dbContext.Addresses.Remove(address);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}