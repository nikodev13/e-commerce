using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Shared.CQRS;
using ECommerce.Application.Users.Exceptions;
using ECommerce.Domain.Shared.ValueObjects;
using ECommerce.Domain.Users.Abstractions;
using ECommerce.Domain.Users.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Users.Commands;

public class RegisterUserCommand : ICommand
{
    public required string Email { get; init; }
    public required string Password { get; init; }
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

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand> 
{
    private readonly IAppDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUserCommandHandler(IAppDbContext dbContext, IPasswordHasher passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }
    
    public async Task<Unit> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.Users.AnyAsync(x => x.Email == new Email(request.Email), cancellationToken))
            throw new EmailAlreadyInUseException(request.Email);
        
        var user = User.CreateRegistered(request.Email, request.Password, _passwordHasher);
        
        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}