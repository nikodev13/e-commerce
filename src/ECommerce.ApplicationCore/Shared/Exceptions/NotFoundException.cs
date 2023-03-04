namespace ECommerce.ApplicationCore.Shared.Exceptions;

public class NotFoundException : ApplicationException
{
    public NotFoundException(string message) : base(message) { }
}