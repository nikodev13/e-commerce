using ECommerce.Application.Shared.Exceptions;

namespace ECommerce.Application.Users.Exceptions;

public class UserAlreadyLoggedInException : BadRequestException
{
    public UserAlreadyLoggedInException() : base("You are already logged in.") { }
}