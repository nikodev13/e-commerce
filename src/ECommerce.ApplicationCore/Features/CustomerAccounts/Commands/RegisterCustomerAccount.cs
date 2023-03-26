using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ECommerce.ApplicationCore.Features.CustomerAccounts.Commands;

public record RegisterCustomerAccountCommand(string FirstName, string LastName, string Email, string PhoneNumber) : ICommand;

public class RegisterCustomerAccountCommandHandler : ICommandHandler<RegisterCustomerAccountCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;
    private readonly ILogger<RegisterCustomerAccountCommand> _logger;

    public RegisterCustomerAccountCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider, ILogger<RegisterCustomerAccountCommand> logger)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
        _logger = logger;
    }
    
    public async ValueTask HandleAsync(RegisterCustomerAccountCommand accountCommand, CancellationToken cancellationToken)
    {
        var customerAccount = new CustomerAccount
        {
            Id = _userContextProvider.UserId!.Value,
            FirstName = accountCommand.FirstName,
            LastName = accountCommand.LastName,
            Email = accountCommand.Email,
            PhoneNumber = accountCommand.PhoneNumber,
        };

        await _dbContext.CustomersAccounts.AddAsync(customerAccount, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("User with id {0} completed customer account registration", _userContextProvider.UserId);
    }
}

public class RegisterUserAccountCommandValidator : AbstractValidator<RegisterCustomerAccountCommand>
{
    public RegisterUserAccountCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        // TODO: ADD REGEX TO PHONE NUMBER VALIDATION
        RuleFor(x => x.PhoneNumber).NotEmpty();
    }
}