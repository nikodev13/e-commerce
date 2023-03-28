using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Orders.Exceptions;

public class OrderLineNotFoundException : NotFoundException
{
    public OrderLineNotFoundException(long orderId, long productId) 
        : base($"Order line for order with id `{orderId}` and product with id `{productId}` not found.") { }
}