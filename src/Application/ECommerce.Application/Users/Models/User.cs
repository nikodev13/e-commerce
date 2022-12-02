namespace ECommerce.Application.Users.Models;

public class User
{
    public Guid Id { get; init; } 
    public string Email { get; init; }
    public Role Role { get; init; }
    public string PasswordHash { get; init; }
}