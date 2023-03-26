using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.CustomerAccounts.Commands;

public record UpdateCustomerFullNameCommand(string FirstName, string LastName) : ICommand;

public class UpdateCustomerFullNameCommandHandler : ICommandHandler<UpdateCustomerFullNameCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public UpdateCustomerFullNameCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask HandleAsync(UpdateCustomerFullNameCommand command, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;
        var account = await _dbContext.CustomersAccounts.FirstOrDefaultAsync(x => x.Id == customerId, cancellationToken);
        
        account!.FirstName = command.FirstName;
        account.LastName = command.LastName;
            
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public class UpdateCustomerFullNameCommandValidator : AbstractValidator<UpdateCustomerFullNameCommand>
{
    public UpdateCustomerFullNameCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}