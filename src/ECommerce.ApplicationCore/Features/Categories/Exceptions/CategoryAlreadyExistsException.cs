using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Categories.Exceptions;

public class CategoryAlreadyExistsException : AlreadyExistsException
{
    public CategoryAlreadyExistsException(string categoryName) : base($"Category with name `{categoryName}` already exists.") { }
}