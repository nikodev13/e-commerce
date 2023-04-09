using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Categories.Exceptions;

public class CategoryWithProductsCannotBeDeletedException : BadRequestException
{
    public CategoryWithProductsCannotBeDeletedException() : base("Category with products can not be deleted.") { }
}