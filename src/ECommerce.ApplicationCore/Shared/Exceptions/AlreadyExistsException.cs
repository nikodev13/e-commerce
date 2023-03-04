namespace ECommerce.ApplicationCore.Shared.Exceptions;

public class AlreadyExistsException : ApplicationException
{
    public AlreadyExistsException(string message) : base(message) { }
}