namespace ECommerce.Application.Common.Results.Errors;

public class BadRequestError : ErrorBase
{
    public BadRequestError(string message) : base(message)
    {
    }
}