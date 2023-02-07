using ECommerce.Application.Shared.Exceptions;

namespace ECommerce.Application.Management.Categories.Exceptions;

public class CategoryAlreadyExistsException : AlreadyExistsException
{
    public CategoryAlreadyExistsException(string categoryName) : base($"Category with name `{categoryName}` already exists.") { }
}