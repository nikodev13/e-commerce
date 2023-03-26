using ECommerce.ApplicationCore.Features.Users.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Users.Commands;

public record RefreshTokenCommand(string Email, string RefreshToken) : ICommand<TokensReadModel>;

internal sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, TokensReadModel>
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
        var (email, refreshToken) = request; 
        
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email.ToLower(), cancellationToken);
        if (user?.RefreshToken is null || user.RefreshToken != refreshToken) throw new InvalidRefreshTokenCredentialsException();

        var newAccessToken = _tokenProvider.GenerateAccessToken(user);
        var newRefreshToken = _tokenProvider.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TokensReadModel(newAccessToken, newRefreshToken);
    }
}

internal sealed class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
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