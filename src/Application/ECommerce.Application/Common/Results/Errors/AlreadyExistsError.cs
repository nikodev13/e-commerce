namespace ECommerce.Application.Common.Results.Errors;

public class AlreadyExistsError : ErrorBase
{
    public AlreadyExistsError(string message) : base(message)
    {
    }
}