using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Customer.Adresses.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Customer.Adresses.Commands;

public class AddNewAddressCommand : ICommand<long>
{
    public required string Street { get; init; }
    public required string PostalCode { get; init; }
    public required string City { get; init; }
}

public class AddNewAddressCommandHandler : ICommandHandler<AddNewAddressCommand, long>
{
    
    private readonly IAppDbContext _dbContext;
    private readonly ISnowflakeIdProvider _snowflakeIdProvider;
    private readonly IUserContextProvider _userContextProvider;

    public AddNewAddressCommandHandler(IAppDbContext dbContext, ISnowflakeIdProvider snowflakeIdProvider, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _snowflakeIdProvider = snowflakeIdProvider;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask<long> HandleAsync(AddNewAddressCommand command, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;
        
        var numberOfCustomerAddresses = await _dbContext.Addresses.Where(x => x.CustomerId == customerId).CountAsync(cancellationToken);
        if (numberOfCustomerAddresses >= 3)
            throw new MoreThanThreeAddressesException();

        var address = new Address
        {
            Id = _snowflakeIdProvider.GenerateId(),
            CustomerId = customerId,
            Street = command.Street,
            PostalCode = command.PostalCode,
            City = command.City
        };

        await _dbContext.Addresses.AddAsync(address, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return address.Id;
    }
}

public class AddNewAddressCommandValidator : AbstractValidator<AddNewAddressCommand>
{
    public AddNewAddressCommandValidator()
    {
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.PostalCode).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
    }
}