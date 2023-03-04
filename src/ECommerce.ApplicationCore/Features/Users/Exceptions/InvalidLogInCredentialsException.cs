using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Users.Exceptions;

public class InvalidLogInCredentialsException : BadRequestException
{
    public InvalidLogInCredentialsException() : base("Invalid email or password.") { }
}