namespace ECommerce.Domain.Users.Abstractions;

public interface IPasswordHasher
{
    public string HashPassword(string password);
    public bool ValidatePassword(string password, string correctHash);
}