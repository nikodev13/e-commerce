namespace ECommerce.Application.Users.Interfaces;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool ValidatePassword(string password, string correctHash);
}