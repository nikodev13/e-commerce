using ECommerce.Application.Shared.Exceptions;

namespace ECommerce.Application.Management.Products.Exceptions;

public class ProductAlreadyExistsException : AlreadyExistsException
{
    public ProductAlreadyExistsException(string productName) : base($"Product with name `{productName}` already exists.") { }
}