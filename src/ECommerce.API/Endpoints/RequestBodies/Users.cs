using ECommerce.ApplicationCore.Features.Users.Commands;

namespace ECommerce.API.Endpoints.RequestBodies;

public record LoginUserRequestBody(string Email, string Password)
{
    public LoginUserCommand ToCommand() => new(Email, Password);
}

public record RegisterUserRequestBody(string Email, string Password)
{
    public RegisterUserCommand ToCommand() => new(Email, Password);
}

public record RefreshTokenRequestBody(string Email, string RefreshToken)
{
    public RefreshTokenCommand ToCommand() => new(Email, RefreshToken);
}
