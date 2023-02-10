using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Shared.CQRS;
using ECommerce.Application.Users.Exceptions;
using ECommerce.Domain.Shared.ValueObjects;
using ECommerce.Domain.Users.Abstractions;
using ECommerce.Domain.Users.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using IUserContextProvider = ECommerce.Application.Shared.Abstractions.IUserContextProvider;

namespace ECommerce.Application.Users.Commands;

public sealed class LoginUserCommand : ICommand<TokensReadModel>
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, TokensReadModel>
{
    private readonly IAppDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenProvider _tokenProvider;
    private readonly IUserContextProvider _userContextProvider;

    public LoginUserCommandHandler(IAppDbContext dbContext,
        IPasswordHasher passwordHasher,
        ITokenProvider tokenProvider,
        IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _tokenProvider = tokenProvider;
        _userContextProvider = userContextProvider;
    }

    public async ValueTask<TokensReadModel> HandleAsync(LoginUserCommand request, CancellationToken cancellationToken)
    {
        if (_userContextProvider.UserId is not null) throw new UserAlreadyLoggedInException();
            
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == new Email(request.Email), cancellationToken: cancellationToken);
        if (user is null) throw new InvalidLogInCredentialsException();

        var (accessToken, refreshToken) = user.LogIn(request.Password, _passwordHasher, _tokenProvider);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return new TokensReadModel()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
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
