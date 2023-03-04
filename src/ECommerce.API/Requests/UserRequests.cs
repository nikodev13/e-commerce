namespace ECommerce.API.Requests;

public class LoginRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class RegisterRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}

public class RefreshTokenRequest
{
    public required string Email { get; init; }
    public required string RefreshToken { get; init; }
}