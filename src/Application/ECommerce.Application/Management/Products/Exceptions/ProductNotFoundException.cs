using ECommerce.Application.Shared.Exceptions;

namespace ECommerce.Application.Management.Products.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(long productId) : base($"Product with id `{productId}` not found.") { }
}