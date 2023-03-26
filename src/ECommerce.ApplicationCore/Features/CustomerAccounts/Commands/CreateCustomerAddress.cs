using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.CustomerAccounts.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.CustomerAccounts.Commands;

public record CreateCustomerAddressCommand(string Street, string PostalCode, string City) : ICommand<long>;

public class CreateCustomerAddressCommandHandler : ICommandHandler<CreateCustomerAddressCommand, long>
{
    
    private readonly IAppDbContext _dbContext;
    private readonly ISnowflakeIdProvider _snowflakeIdProvider;
    private readonly IUserContextProvider _userContextProvider;

    public CreateCustomerAddressCommandHandler(IAppDbContext dbContext, ISnowflakeIdProvider snowflakeIdProvider, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _snowflakeIdProvider = snowflakeIdProvider;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask<long> HandleAsync(CreateCustomerAddressCommand command, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;
        
        var numberOfCustomerAddresses = await _dbContext.Addresses.Where(x => x.CustomerId == customerId).CountAsync(cancellationToken);
        if (numberOfCustomerAddresses >= 3)
            throw new MoreThanThreeAddressesException();

        var address = new CustomerAddress
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

public class CreateAddressCommandValidator : AbstractValidator<CreateCustomerAddressCommand>
{
    public CreateAddressCommandValidator()
    {
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.PostalCode).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
    }
}