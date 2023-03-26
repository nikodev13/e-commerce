using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Orders.Exceptions;

public class ProductOutOfStockException : BadRequestException
{
    public ProductOutOfStockException(long productId, uint availableQuantity) 
        : base($"Available quantity for product with id `{productId}` is {availableQuantity}.") { }
}