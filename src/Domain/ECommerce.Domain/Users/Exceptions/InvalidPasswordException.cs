using ECommerce.Domain.Shared.Exceptions;

namespace ECommerce.Domain.Users.Exceptions;

public class InvalidPasswordException : DomainException
{
    public InvalidPasswordException() : base("Invalid password.") { }
}