namespace ECommerce.Application.Common.Results.Errors;

public class NotFoundError : ErrorBase
{
    public NotFoundError(string message) : base(message)
    {
    }
}