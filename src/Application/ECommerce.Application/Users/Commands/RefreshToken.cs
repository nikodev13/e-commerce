using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Users.Interfaces;
using ECommerce.Application.Users.ReadModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Users.Commands;

public class RefreshTokenCommand : ICommand<TokensReadModel>
{
    public string RefreshToken { get; }

    public RefreshTokenCommand(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, TokensReadModel>
{
    private readonly IApplicationDatabase _database;
    private readonly ITokenProvider _tokenProvider;

    public RefreshTokenCommandHandler(IApplicationDatabase database, ITokenProvider tokenProvider)
    {
        _database = database;
        _tokenProvider = tokenProvider;
    }
    
    public async Task<TokensReadModel> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _database.Users.FirstOrDefaultAsync(x => x.RefreshToken == request.RefreshToken, cancellationToken);
        if (user is null)
        {
            throw new BadRequestException("Invalid refresh token.");
        }

        var accessToken = _tokenProvider.GenerateAccessToken(user);
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