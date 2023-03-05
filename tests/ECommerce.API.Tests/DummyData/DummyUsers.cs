using ECommerce.ApplicationCore.Entities;
using ECommerce.Infrastructure.Authentication;

namespace ECommerce.API.Tests.DummyData;

public class DummyUsers
{
    public static List<User> Data { get; }

    static DummyUsers()
    {
        var passwordHasher = new PasswordHasher();
        
        Data = new List<User>
        {
            new()
            {
                Id = Guid.Parse("45581785-2879-4D82-9037-227D6FE19A76"),
                Email = "outlook@outlook.com",
                PasswordHash = passwordHasher.HashPassword("password1")
            },
            new()
            {
                Id = Guid.Parse("BD88E2A6-EC86-4A2C-9BD3-67CF403725DB"),
                Email = "gmail@gmail.com",
                PasswordHash = passwordHasher.HashPassword("password2")
            },
            new()
            {
                Id = Guid.Parse("466F0805-93D5-47B5-BFA9-1776473C343A"),
                Email = "proton@proton.me",
                PasswordHash = passwordHasher.HashPassword("password3")
            }
        };
    }
}