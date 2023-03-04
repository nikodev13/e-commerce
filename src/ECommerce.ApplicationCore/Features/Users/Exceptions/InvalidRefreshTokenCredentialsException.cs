using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Users.Exceptions;

public class InvalidRefreshTokenCredentialsException : BadRequestException
{
    public InvalidRefreshTokenCredentialsException() : base("Invalid email or refresh token.") { }
}