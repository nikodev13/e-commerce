using ECommerce.ApplicationCore.Features.Users;
using ECommerce.ApplicationCore.Features.Users.Commands;
using ECommerce.ApplicationCore.Features.Users.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Tests.Users;

[Collection(TestingCollection.Name)]
public class RefreshToken
{
    private readonly Testing _testing;

    public RefreshToken(Testing testing)
    {
        _testing = testing;
    }

    [Fact]
    public async Task RefreshToken_WithInvalidEmail_ThrowsInvalidRefreshTokenCredentialsException()
    {
        // arrange
        var command = new RefreshTokenCommand
        {
            Email = "any_invalid@email.com",
            RefreshToken = "any token"
        };

        // act
        async Task<TokensReadModel> Action() => await _testing.ExecuteCommandAsync<RefreshTokenCommand, TokensReadModel>(command);

        // assert
        await Assert.ThrowsAsync<InvalidRefreshTokenCredentialsException>(Action);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("any invalid token")]
    public async Task RefreshToken_WithInvalidRefreshToken_ThrowsInvalidRefreshTokenCredentialsException(string? refreshToken)
    {
        // arrange
        var command = new RefreshTokenCommand
        {
            Email = DummyUsers.Data[0].Email,
            RefreshToken = refreshToken!
        };

        // act
        async Task<TokensReadModel> Action() => await _testing.ExecuteCommandAsync<RefreshTokenCommand, TokensReadModel>(command);

        // assert
        await Assert.ThrowsAsync<InvalidRefreshTokenCredentialsException>(Action);
    }
    
    [Fact]
    public async Task RefreshToken_Successfully_ReturnTokensReadModel()
    {
        // arrange
        var db = _testing.GetAppDbContext();
        var user = await db.Users.FirstAsync(x => x.Email == DummyUsers.Data[0].Email);
        user.RefreshToken = "any refresh token";
        await db.SaveChangesAsync(default);
        
        var command = new RefreshTokenCommand
        {
            Email = user.Email,
            RefreshToken = user.RefreshToken
        };

        // act
        var result = await _testing.ExecuteCommandAsync<RefreshTokenCommand, TokensReadModel>(command);

        // assert
        Assert.NotEmpty(result.AccessToken);
    }
}