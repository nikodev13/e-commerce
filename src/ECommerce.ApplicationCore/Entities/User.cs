namespace ECommerce.ApplicationCore.Entities;

public class User
{
    public required Guid Id { get; init; }
    public required string Email { get; set; }
    public Role Role { get; } = Role.User;
    public DateTime RegisteredAt { get; } = DateTime.Now;

    public required string PasswordHash { get; set; }
    public string? RefreshToken { get; set; }
}

public enum Role
{
    User = 0,
    Admin = 1
}