namespace ECommerce.Application.Common.Results.Errors;

public class BadRequestError : ErrorBase
{
    public override string Message { get; }

    public BadRequestError(string message)
    {
        Message = message;
    }
}