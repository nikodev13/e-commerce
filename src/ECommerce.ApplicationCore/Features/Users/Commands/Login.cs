using ECommerce.ApplicationCore.Features.Users.Exceptions;
using ECommerce.ApplicationCore.Features.Users.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using IUserContextProvider = ECommerce.ApplicationCore.Shared.Abstractions.IUserContextProvider;

namespace ECommerce.ApplicationCore.Features.Users.Commands;

public record LoginUserCommand(string Email, string Password) : ICommand<TokensReadModel>;

internal sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, TokensReadModel>
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
            
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken: cancellationToken);
        if (user is null || !_passwordHasher.ValidatePassword(request.Password, user.PasswordHash)) throw new InvalidLogInCredentialsException();

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var refreshToken = _tokenProvider.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TokensReadModel(accessToken, refreshToken);
    }
}

internal sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
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
