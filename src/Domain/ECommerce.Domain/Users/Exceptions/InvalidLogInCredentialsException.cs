using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Users.Exceptions;

public class InvalidLogInCredentialsException : DomainException
{
    public InvalidLogInCredentialsException() : base("Invalid email or password.") { }
}