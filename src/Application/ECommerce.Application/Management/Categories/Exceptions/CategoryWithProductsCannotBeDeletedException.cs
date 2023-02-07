using ECommerce.Application.Shared.Exceptions;

namespace ECommerce.Application.Management.Categories.Exceptions;

public class CategoryWithProductsCannotBeDeletedException : BadRequestException
{
    public CategoryWithProductsCannotBeDeletedException() : base("Category with products can not be deleted.") { }
}