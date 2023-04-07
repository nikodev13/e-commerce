using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.Orders.ReadModels;

public record OrderReadModel(long Id,
    string OrderStatus,
    PaymentReadModel Payment,
    DeliveryReadModel Delivery,
    List<OrderLineReadModel> OrderLines,
    DateTime PlacedAt,
    DateTime? SentAt)
{
    public static OrderReadModel From(Order order)
        => new(order.Id,
            order.Status.ToString(),
            PaymentReadModel.From(order.Payment),
            DeliveryReadModel.From(order.Delivery),
            order.OrderLines.Select(OrderLineReadModel.From).ToList(),
            order.PlacedAt,
            order.SentAt);
}