using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Users.Interfaces;
using ECommerce.Application.Users.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Users.Commands;

public class RegisterUserCommand : ICommand
{
    public string Email { get; }
    public string Password { get; }
    public string ConfirmPassword { get; }

    public RegisterUserCommand(string email, string password, string confirmPassword)
    {
        Email = email.ToLower();
        Password = password;
        ConfirmPassword = confirmPassword;
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
        RuleFor(x => x.ConfirmPassword)
            .MinimumLength(8);
    }
}

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand> 
{
    private readonly IApplicationDatabase _database;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IApplicationDatabase database, IPasswordHasher passwordHasher)
    {
        _database = database;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (request.Password != request.ConfirmPassword)
            throw new BadRequestException("Confirm password is not same as password.");
            
        if (await _database.Users.AnyAsync(x => x.Email == request.Email, cancellationToken: cancellationToken))
            throw new BadRequestException("User with that email already exists.");

        var passwordHash = _passwordHasher.HashPassword(request.Password);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            IsEmailConfirmed = false,
            PasswordHash = passwordHash,
            Role = Role.User
        };

        await _database.Users.AddAsync(user, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}