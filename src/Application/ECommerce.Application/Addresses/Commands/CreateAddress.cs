using System.Xml.Schema;
using ECommerce.Application.Addresses.ReadModels;
using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Domain.Addresses;
using ECommerce.Domain.Addresses.ValueObjects;
using ECommerce.Domain.Shared.Services;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Addresses.Commands;

public class CreateAddressCommand : ICommand<AddressReadModel>
{
    public string Street { get; }
    public string PostalCode { get; }
    public string City { get; }
    
    public CreateAddressCommand(string street, string postalCode, string city)
    {
        Street = street;
        PostalCode = postalCode;
        City = city;
    }
}

public class CreateAddressCommandHandler : ICommandHandler<CreateAddressCommand, AddressReadModel>
{
    private readonly IApplicationDatabase _database;
    private readonly IUserContextService _userContextService;
    private readonly ISnowflakeIdService _idService;

    public CreateAddressCommandHandler(IApplicationDatabase database, IUserContextService userContextService, ISnowflakeIdService idService)
    {
        _database = database;
        _userContextService = userContextService;
        _idService = idService;
    }
    
    public async Task<AddressReadModel> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
    {
        var userId = _userContextService.UserId!;

        if (await _database.Addresses.Where(x => x.CustomerId.Value == userId).CountAsync(cancellationToken) == 3)
            throw new BadRequestException("You already have a maximum number of addresses.");
            
        if (await _database.Addresses.AnyAsync(x => x.CustomerId.Value == userId
                                                    && x.Street.Value == request.Street
                                                    && x.PostalCode.Value == request.PostalCode
                                                    && x.City.Value == request.City, cancellationToken))
            throw new AlreadyExistsException("Address already exists.");
            
        var address = Address.CreateNew(userId,
            new Street(request.Street),
            new PostalCode(request.PostalCode),
            new City(request.City),
            _idService);


        var result = AddressReadModel.FromAddress(address);
        return result;
    }
}