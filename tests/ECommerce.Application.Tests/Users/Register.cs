using ECommerce.ApplicationCore.Features.Users.Commands;
using ECommerce.ApplicationCore.Features.Users.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Tests.Users;

[Collection(TestingCollection.Name)]
public class Register
{
    private readonly Testing _testing;

    public Register(Testing testing)
    {
        _testing = testing;
    }

    [Fact]
    public async Task UserRegister_Successfully()
    {
        // arrange
        var db = _testing.GetAppDbContext();
        var command = new RegisterUserCommand
        {
            Email = "unique@email.com",
            Password = "password1"
        };

        // act
        await _testing.ExecuteCommandAsync(command);
        var user = await db.Users.FirstOrDefaultAsync(x => x.Email == command.Email);

        // assert
        Assert.Equal(command.Email, user?.Email);
        
        // clean
        await db.Users.Where(x => x.Email == command.Email).ExecuteDeleteAsync();
    }
    
    [Fact]
    public async Task UserRegister_WithEmailThatAlreadyExists_ThrowsBadRequest()
    {
        // arrange
        var db = _testing.GetAppDbContext();
        var command = new RegisterUserCommand
        {
            Email = (await db.Users.FirstAsync()).Email,
            Password = "password1"
        };

        // act
        async Task Action() => await _testing.ExecuteCommandAsync(command);

        // assert
        await Assert.ThrowsAsync<EmailAlreadyInUseException>(Action);
    }
}