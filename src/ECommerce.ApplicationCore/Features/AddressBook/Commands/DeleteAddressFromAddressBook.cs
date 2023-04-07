using ECommerce.ApplicationCore.Features.AddressBook.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.AddressBook.Commands;

public record DeleteAddressFromAddressBookCommand(long Id) : ICommand;

internal sealed class DeleteAddressFromAddressBookCommandHandler : ICommandHandler<DeleteAddressFromAddressBookCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public DeleteAddressFromAddressBookCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask HandleAsync(DeleteAddressFromAddressBookCommand fromAddressBookCommand, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;
        var address = await _dbContext.Addresses.FirstOrDefaultAsync(x => x.Id == fromAddressBookCommand.Id && x.CustomerId == customerId, cancellationToken);
        if (address is null)
            throw new AddressNotFoundException(fromAddressBookCommand.Id);

        _dbContext.Addresses.Remove(address);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}