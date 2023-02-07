using ECommerce.Application.Shared.Exceptions;

namespace ECommerce.Application.Users.Exceptions;

public class EmailAlreadyInUseException : BadRequestException
{
    public EmailAlreadyInUseException(string email) : base($"User with email `{email}` already exists.") { }
}