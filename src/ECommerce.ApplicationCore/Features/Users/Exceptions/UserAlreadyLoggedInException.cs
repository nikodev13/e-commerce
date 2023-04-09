using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Users.Exceptions;

public class UserAlreadyLoggedInException : BadRequestException
{
    public UserAlreadyLoggedInException() : base("You are already logged in.") { }
}