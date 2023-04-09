using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Customers.Commands;

public record UpdateCustomerContactDataCommand(string Email, string PhoneNumber) : ICommand;

internal sealed class UpdateCustomerContactDataCommandHandler : ICommandHandler<UpdateCustomerContactDataCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public UpdateCustomerContactDataCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask HandleAsync(UpdateCustomerContactDataCommand command, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;
        var customer = await _dbContext.Customers
            .FirstOrDefaultAsync(x => x.Id == customerId, cancellationToken);

        customer!.Email = command.Email;
        customer.PhoneNumber = command.PhoneNumber;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

internal sealed class UpdateCustomerContactDataCommandValidator : AbstractValidator<UpdateCustomerContactDataCommand>
{
    public UpdateCustomerContactDataCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty();
    }
}