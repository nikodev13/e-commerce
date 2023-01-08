using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Addresses.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Addresses.Commands;

public class UpdateAddressCommand : ICommand 
{
    public long Id { get; }
    public string Street { get; }
    public string PostalCode { get; }
    public string City { get; }
    
    public UpdateAddressCommand(long id, string street, string postalCode, string city)
    {
        Id = id;
        Street = street;
        PostalCode = postalCode;
        City = city;
    }
}

public class UpdateAddressCommandHandler : ICommandHandler<UpdateAddressCommand>
{
    private readonly IApplicationDatabase _database;
    private readonly IUserContextService _userContextService;

    public UpdateAddressCommandHandler(IApplicationDatabase database, IUserContextService userContextService)
    {
        _database = database;
        _userContextService = userContextService;
    }
    
    public async Task<Unit> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContextService.UserId!;

        if (await _database.Addresses.AnyAsync(x => x.CustomerId.Value == userId
                                                    && x.Street.Value == request.Street
                                                    && x.PostalCode.Value == request.PostalCode
                                                    && x.City.Value == request.City, cancellationToken))
            throw new AlreadyExistsException("Address already exists.");

        var address = await _database.Addresses.FirstOrDefaultAsync(x => x.Id == request.Id && x.CustomerId.Value == userId, cancellationToken);
        if (address is null)
            throw new NotFoundException($"Address with ID `{request.Id}` not found.");

        address.Street = new Street(request.Street);
        address.PostalCode = new PostalCode(request.Street);
        address.City = new City(request.Street);

        await _database.SaveChangesAsync(cancellationToken);
        
        return Unit.Value;
    }
}