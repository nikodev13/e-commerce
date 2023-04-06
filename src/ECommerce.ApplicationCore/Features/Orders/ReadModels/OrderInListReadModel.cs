using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.Orders.ReadModels;

public record OrderInListReadModel(long Id, string OrderStatus, DateTime PlacedAt)
{
    public static OrderInListReadModel From(Order order)
        => new(order.Id, order.Status.ToString(), order.PlacedAt);
}