using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Users.Commands;

public record ChangeUserEmailCommand(string NewMail) : ICommand;

internal sealed class ChangeUserEmailCommandHandler : ICommandHandler<ChangeUserEmailCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public ChangeUserEmailCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask HandleAsync(ChangeUserEmailCommand command, CancellationToken cancellationToken)
    {
        var userId = _userContextProvider.UserId!.Value;
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        
        user!.Email = command.NewMail;
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

internal sealed class ChangeUserEmailCommandValidator : AbstractValidator<ChangeUserEmailCommand>
{
    public ChangeUserEmailCommandValidator()
    {
        RuleFor(x => x.NewMail).NotEmpty();
    }
}
