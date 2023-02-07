using ECommerce.Application.Shared.Exceptions;

namespace ECommerce.Application.Management.Categories.Exceptions;

public class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(long categoryId) : base($"Category with id `{categoryId}` not found.") { }
}