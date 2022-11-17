namespace ECommerce.Application.Shared.Results.Errors;

public class NotFoundError : ErrorBase
{
    public override string Message { get; }

    public NotFoundError(string message)
    {
        Message = message;
    }
}