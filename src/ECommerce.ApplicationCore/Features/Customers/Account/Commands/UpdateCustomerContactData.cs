using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Customers.Account.Commands;

public class UpdateCustomerContactDataCommand : ICommand
{
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
}

public class UpdateCustomerContactDataCommandHandler : ICommandHandler<UpdateCustomerContactDataCommand>
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
        var account = await _dbContext.CustomersAccounts.FirstOrDefaultAsync(x => x.Id == customerId, cancellationToken);

        account!.Email = command.Email;
        account.PhoneNumber = command.PhoneNumber;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public class UpdateCustomerContactDataCommandValidator : AbstractValidator<UpdateCustomerContactDataCommand>
{
    public UpdateCustomerContactDataCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.PhoneNumber).NotEmpty();
    }
}