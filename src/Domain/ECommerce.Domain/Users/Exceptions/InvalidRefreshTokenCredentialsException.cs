using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Users.Exceptions;

public class InvalidRefreshTokenCredentialsException : DomainException
{
    public InvalidRefreshTokenCredentialsException() : base("Invalid email or refresh token.") { }
}