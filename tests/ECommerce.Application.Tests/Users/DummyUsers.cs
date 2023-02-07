using ECommerce.Domain.Users.Entities;
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
            User.CreateRegistered("john@doe.com", "password1", passwordHasher),
            User.CreateRegistered("test@test.pl", "password2", passwordHasher),
            User.CreateRegistered("zsl@zsl.edu", "password3", passwordHasher),
        };
    }
}