
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Addresses.Commands;

public class DeleteAddressCommand : ICommand
{
    public long Id { get; }

    public DeleteAddressCommand(long id)
    {
        Id = id;
    }
}

public class DeleteAddressCommandHandler : ICommandHandler<DeleteAddressCommand>
{
    private readonly IApplicationDatabase _database;
    private readonly IUserContextService _userContextService;

    public DeleteAddressCommandHandler(IApplicationDatabase database, IUserContextService userContextService)
    {
        _database = database;
        _userContextService = userContextService;
    }
    
    public async Task<Unit> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContextService.UserId!;
        
        var address = await _database.Addresses.FirstOrDefaultAsync(x => x.Id == request.Id && x.CustomerId.Value == userId, cancellationToken);
        if (address is null)
            throw new NotFoundException($"Address with ID `{request.Id}` not found.");

        _database.Addresses.Remove(address);
        await _database.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}