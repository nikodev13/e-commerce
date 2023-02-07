using ECommerce.Domain.Shared;
using ECommerce.Domain.Shared.Exceptions;
using ECommerce.Domain.Shared.ValueObjects;
using ECommerce.Domain.Users.Abstractions;
using ECommerce.Domain.Users.Exceptions;
using ECommerce.Domain.Users.ValueObjects;

namespace ECommerce.Domain.Users.Entities;

public class User : Entity
{
    public required UserId Id { get; init; }
    public Email Email { get; private set; } = default!;
    public Role Role { get; init; } = default!;
    public DateTime RegisteredAt { get; init; }

    private string _passwordHash = default!;
    private string? _refreshToken;
    
    private User() { }
    
    public static User CreateRegistered(string email, string password, IPasswordHasher passwordHasher)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = new Email(email),
            Role = Role.User,
            RegisteredAt = DateTime.Now,
            _passwordHash = passwordHasher.HashPassword(password)
        };
        
        return user;
    }

    public void UpdateEmail(string email)
    {
        Email = new Email(email);
    }

    public (string AccessToken, string RefreshToken) LogIn(string password, IPasswordHasher passwordHasher, ITokenProvider tokenProvider)
    {
        if (!passwordHasher.ValidatePassword(password, _passwordHash)) throw new InvalidLogInCredentialsException();

        _refreshToken = tokenProvider.GenerateRefreshToken();
        
        return (tokenProvider.GenerateAccessToken(this), _refreshToken);
    }

    public void LogOut()
    {
        _refreshToken = null;
    }

    public (string AccessToken, string RefreshToken) RefreshTokens(string currentRefreshToken, ITokenProvider tokenProvider)
    {
        if (_refreshToken != currentRefreshToken) throw new InvalidRefreshTokenCredentialsException();
        
        _refreshToken = tokenProvider.GenerateRefreshToken();

        return (tokenProvider.GenerateAccessToken(this), _refreshToken);
    }

    public void ChangePassword(string currentPassword, string newPassword, IPasswordHasher passwordHasher)
    {
        if (!passwordHasher.ValidatePassword(currentPassword, _passwordHash)) throw new InvalidPasswordException();

        _passwordHash = passwordHasher.HashPassword(newPassword);
    }
}