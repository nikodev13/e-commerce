namespace ECommerce.Application.Users.Models;

public class User
{
    public Guid Id { get; init; } 
    public string Email { get; init; }
    public bool IsEmailConfirmed { get; set; }
    public string PasswordHash { get; init; }
    public Role Role { get; init; }
    
    public string? RefreshToken { get; set; }
}