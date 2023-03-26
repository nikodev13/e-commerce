using ECommerce.ApplicationCore.Features.Users.Commands;

namespace ECommerce.API.Endpoints.Requests;

public record LoginUserRequest(string Email, string Password)
{
    public LoginUserCommand ToCommand() => new(Email, Password);
}

public record RegisterUserRequest(string Email, string Password)
{
    public RegisterUserCommand ToCommand() => new(Email, Password);
}

public record RefreshTokenRequest(string Email, string RefreshToken)
{
    public RefreshTokenCommand ToCommand() => new(Email, RefreshToken);
}
