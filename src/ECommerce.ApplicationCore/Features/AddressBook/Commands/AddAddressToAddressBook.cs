using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.AddressBook.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.AddressBook.Commands;

public record AddAddressToAddressBookCommand(string Street, string PostalCode, string City) : ICommand<long>;

internal sealed class AddAddressToAddressBookCommandHandler : ICommandHandler<AddAddressToAddressBookCommand, long>
{
    private readonly IAppDbContext _dbContext;
    private readonly ISnowflakeIdProvider _snowflakeIdProvider;
    private readonly IUserContextProvider _userContextProvider;

    public AddAddressToAddressBookCommandHandler(IAppDbContext dbContext, ISnowflakeIdProvider snowflakeIdProvider, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _snowflakeIdProvider = snowflakeIdProvider;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask<long> HandleAsync(AddAddressToAddressBookCommand toAddressBookCommand, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;
        
        var numberOfCustomerAddresses = await _dbContext.Addresses.Where(x => x.CustomerId == customerId).CountAsync(cancellationToken);
        if (numberOfCustomerAddresses >= 3)
            throw new MoreThanThreeAddressesException();

        var address = new Address
        {
            Id = _snowflakeIdProvider.GenerateId(),
            CustomerId = customerId,
            Street = toAddressBookCommand.Street,
            PostalCode = toAddressBookCommand.PostalCode,
            City = toAddressBookCommand.City
        };

        await _dbContext.Addresses.AddAsync(address, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return address.Id;
    }
}

internal sealed class CreateAddressCommandValidator : AbstractValidator<AddAddressToAddressBookCommand>
{
    public CreateAddressCommandValidator()
    {
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.PostalCode).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
    }
}