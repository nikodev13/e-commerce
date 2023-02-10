using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Shared.CQRS;
using ECommerce.Domain.Users.Abstractions;
using ECommerce.Domain.Users.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Users.Commands;

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
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email.Value == request.Email.ToLower(), cancellationToken);
        if (user is null) throw new InvalidRefreshTokenCredentialsException();

        var (accessToken, refreshToken) = user.RefreshTokens(request.RefreshToken, _tokenProvider);
        
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