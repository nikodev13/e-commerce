using ECommerce.ApplicationCore.Entities;
using ECommerce.Infrastructure.Authentication;

namespace ECommerce.Application.Tests.Users;

public static class DummyUsers
{
    public static List<User> Data { get; }
    static DummyUsers()
    {
        var passwordHasher = new PasswordHasher();
        
        Data = new List<User>
        {
            new()
            {
                Id = Guid.Parse("9885C140-D2BA-43FC-BAE5-5E8E5BE236B0"),
                Email = "john@doe.com",
                PasswordHash = passwordHasher.HashPassword("password1"),
            },
            new()
            {
                Id = Guid.Parse("724137F7-D6F5-4B99-96C0-CE0A1973900C"),
                Email = "test@test.pl",
                PasswordHash = passwordHasher.HashPassword("password2"),
            },
            new()
            {
                Id = Guid.Parse("D16171D4-D62A-496C-A126-7FEC00150258"),
                Email = "zsl@zsl.edu",
                PasswordHash = passwordHasher.HashPassword("password3"),
            }
        };
    }
}