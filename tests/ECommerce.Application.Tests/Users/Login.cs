using ECommerce.Application.Tests.Utilities;
using ECommerce.ApplicationCore.Features.Users;
using ECommerce.ApplicationCore.Features.Users.Commands;
using ECommerce.ApplicationCore.Features.Users.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Tests.Users;

[Collection(TestingCollection.Name)]
public class Login
{
    private readonly Testing _testing;

    public Login(Testing testing)
    {
        _testing = testing;
    }

    [Fact]
    public async Task Login_Successfully_ReturnAccessAndRefreshToken()
    {
        // arrange 
        var db = _testing.GetAppDbContext();
        var user = await db.Users.FirstAsync(x => x.Email == DummyUsers.Data[0].Email);
        var command = new LoginUserCommand(user.Email, "password1");

        // act
        var result = await _testing.ExecuteCommandAsync<LoginUserCommand, TokensReadModel>(command);
        user = await db.Users.FirstAsync(x => x.Email == DummyUsers.Data[0].Email);;
        
        // assert
        Assert.NotEmpty(result.RefreshToken);
        Assert.Equal(user.RefreshToken, result.RefreshToken);
    }
    
    [Fact]
    public async Task Login_ByAuthenticatedUser_ThrowsUserAlreadyLoggedInException()
    {
        // arrange 
        var user = DummyUsers.Data[0];
        FakeUserContextProvider.CurrentUserId = user.Id;
        var command = new LoginUserCommand(user.Email, "password1");

        // act
        async Task<TokensReadModel> Action() => await _testing.ExecuteCommandAsync<LoginUserCommand, TokensReadModel>(command);
        
        // assert
        await Assert.ThrowsAsync<UserAlreadyLoggedInException>(Action);
        
        // clean
        FakeUserContextProvider.CurrentUserId = null;
    }
    
    [Fact]
    public async Task Login_WithInvalidPassword_ThrowsInvalidLogInCredentialsException()
    {
        // arrange 
        var user = DummyUsers.Data[0];
        var command = new LoginUserCommand(user.Email, "password2");

        // act
        async Task<TokensReadModel> Action() => await _testing.ExecuteCommandAsync<LoginUserCommand, TokensReadModel>(command);

        // assert
        await Assert.ThrowsAsync<InvalidLogInCredentialsException>(Action);
    }
}