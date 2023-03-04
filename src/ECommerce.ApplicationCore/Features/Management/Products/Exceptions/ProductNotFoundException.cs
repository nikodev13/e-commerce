using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Management.Products.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(long productId) : base($"Product with id `{productId}` not found.") { }
}