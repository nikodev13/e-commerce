using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Results.Errors;
using ECommerce.Application.Users.Interfaces;
using ECommerce.Application.Users.Models;
using ECommerce.Application.Users.ReadModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Users.Commands;

public class LoginUserCommand : ICommand<TokensReadModel>
{
    public string Email { get; }
    public string Password { get; }

    public LoginUserCommand(string email, string password)
    {
        Email = email.ToLower();
        Password = password;
    }
}

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        RuleFor(x => x.Password)
            .MinimumLength(8);
    }
}

public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, TokensReadModel>
{
    private readonly IApplicationDatabase _database;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserContextService _userContextService;

    public LoginUserCommandHandler(IApplicationDatabase database,
        IPasswordHasher passwordHasher,
        ITokenProvider tokenProvider,
        IUserContextService userContextService)
    {
        _database = database;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
        _userContextService = userContextService;
    }

    public async Task<Result<TokensReadModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        if (_userContextService.UserId is not null)
            return new BadRequestError("You're already logged in.");
            
        var user = await _database.Users.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken: cancellationToken);
        if (user is null)
            return new BadRequestError("Invalid email or password.");

        if (!_passwordHasher.ValidatePassword(request.Password, user.PasswordHash))
            return new BadRequestError("Invalid email or password.");
        
        var accessToken =_tokenProvider.GenerateAccessToken(user);
        var refreshToken = _tokenProvider.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        await _database.SaveChangesAsync(cancellationToken);
        
        return new TokensReadModel()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}
