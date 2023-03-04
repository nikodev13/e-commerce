namespace ECommerce.ApplicationCore.Shared.Exceptions;

public class BadRequestException : ApplicationException
{
    public BadRequestException(string message) : base(message) { }
}