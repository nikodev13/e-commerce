using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.Orders.ReadModels;

public record OrderLineReadModel(long ProductId, string ProductName, uint Quantity, decimal UnitPrice)
{
    public static OrderLineReadModel From(OrderLine orderLine)
        => new(orderLine.ProductId, orderLine.Product.Name, orderLine.Quantity, orderLine.UnitPrice);
}