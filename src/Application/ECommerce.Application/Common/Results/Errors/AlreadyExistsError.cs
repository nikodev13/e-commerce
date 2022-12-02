namespace ECommerce.Application.Common.Results.Errors;

public class AlreadyExistsError : ErrorBase
{
    public override string Message { get; }

    public AlreadyExistsError(string message)
    {
        Message = message;
    }
}