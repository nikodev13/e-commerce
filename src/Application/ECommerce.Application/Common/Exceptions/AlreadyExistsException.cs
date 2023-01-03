using System.ComponentModel;

namespace ECommerce.Application.Common.Exceptions;

public class AlreadyExistsException : Exception
{
    public AlreadyExistsException(string message) : base(message)
    {
        
    }
}