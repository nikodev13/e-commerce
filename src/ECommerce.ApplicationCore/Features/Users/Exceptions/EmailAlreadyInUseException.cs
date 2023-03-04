using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Users.Exceptions;

public class EmailAlreadyInUseException : BadRequestException
{
    public EmailAlreadyInUseException(string email) : base($"User with email `{email}` already exists.") { }
}