using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Products.Exceptions;

public class ProductNotFoundByIdException : NotFoundException
{
    public ProductNotFoundByIdException(long id) : base($"Product with id `{id}` not found.") { }
}