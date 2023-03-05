using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ECommerce.ApplicationCore.Features.Customers.Account.Commands;

public class RegisterCustomerCommand : ICommand
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
}

public class RegisterCustomerCommandHandler : ICommandHandler<RegisterCustomerCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;
    private readonly ILogger<RegisterCustomerCommand> _logger;

    public RegisterCustomerCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider, ILogger<RegisterCustomerCommand> logger)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
        _logger = logger;
    }
    
    public async ValueTask HandleAsync(RegisterCustomerCommand command, CancellationToken cancellationToken)
    {
        var customerAccount = new CustomerAccount
        {
            Id = _userContextProvider.UserId!.Value,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PhoneNumber = command.PhoneNumber,
        };

        await _dbContext.CustomersAccounts.AddAsync(customerAccount, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("User with id {0} completed customer account registration", _userContextProvider.UserId);
    }
}

public class RegisterUserAccountCommandValidator : AbstractValidator<RegisterCustomerCommand>
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