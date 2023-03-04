using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Users.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Users.Commands;

public sealed class RegisterUserCommand : ICommand
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand> 
{
    private readonly IAppDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IAppDbContext dbContext, IPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }
    
    public async ValueTask HandleAsync(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.ToLower();
        
        if (await _dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken))
            throw new EmailAlreadyInUseException(email);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = _passwordHasher.HashPassword(request.Password),
            RefreshToken = null
        };

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .MinimumLength(8);
    }
}
