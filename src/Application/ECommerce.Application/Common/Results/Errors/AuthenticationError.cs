namespace ECommerce.Application.Common.Results.Errors;

public class AuthenticationError : ErrorBase
{
    public override string Message { get; }

    public AuthenticationError(string message)
    {
        Message = message;
    }
}