using ECommerce.ApplicationCore.Features.CustomerAccounts.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.CustomerAccounts.Commands;

public record UpdateCustomerAddressCommand(long Id, string Street, string PostalCode, string City) : ICommand;

public class UpdateCustomerAddressCommandHandler : ICommandHandler<UpdateCustomerAddressCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public UpdateCustomerAddressCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask HandleAsync(UpdateCustomerAddressCommand command, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;

        var address = await _dbContext.Addresses.FirstOrDefaultAsync(x => x.Id == command.Id && x.CustomerId == customerId, cancellationToken);
        if (address is null)
            throw new AddressNotFoundException(command.Id);

        address.Street = command.Street;
        address.PostalCode = command.PostalCode;
        address.City = command.City;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public class UpdateAddressCommandValidator : AbstractValidator<UpdateCustomerAddressCommand>
{
    public UpdateAddressCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.PostalCode).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
    }
}