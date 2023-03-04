using ECommerce.ApplicationCore.Features.Users.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Users.Commands;

public class RefreshTokenCommand : ICommand<TokensReadModel>
{
    public required string Email { get; init; }
    public required string RefreshToken { get; init; }
}

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, TokensReadModel>
{
    private readonly IAppDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public RefreshTokenCommandHandler(IAppDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }
    
    public async ValueTask<TokensReadModel> HandleAsync(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email.ToLower(), cancellationToken);
        if (user?.RefreshToken is null || user.RefreshToken != request.RefreshToken) throw new InvalidRefreshTokenCredentialsException();

        var accessToken = _tokenProvider.GenerateAccessToken(user);
        var refreshToken = _tokenProvider.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TokensReadModel()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }
}

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();
        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}