using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Customers.Commands;

public record UpdateCustomerFullNameCommand(string FirstName, string LastName) : ICommand;

internal sealed class UpdateCustomerFullNameCommandHandler : ICommandHandler<UpdateCustomerFullNameCommand>
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
        var customer = await _dbContext.Customers
            .FirstOrDefaultAsync(x => x.Id == customerId, cancellationToken);
        
        customer!.FirstName = command.FirstName;
        customer.LastName = command.LastName;
            
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

internal sealed class UpdateCustomerFullNameCommandValidator : AbstractValidator<UpdateCustomerFullNameCommand>
{
    public UpdateCustomerFullNameCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}