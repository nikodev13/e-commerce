namespace ECommerce.Application.Common.Results.Errors;

public abstract class ErrorBase
{
    public string Message { get; }

    protected ErrorBase(string message)
    {
        Message = message;
    }
}