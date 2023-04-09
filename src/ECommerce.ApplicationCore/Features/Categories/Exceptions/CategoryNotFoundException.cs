using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Categories.Exceptions;

public class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(long categoryId) : base($"Category with id `{categoryId}` not found.") { }
}