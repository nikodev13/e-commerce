using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.Orders.ReadModels;

public record DeliveryReadModel(string Operator, string? TrackingNumber, string Street, string City, string PostalCode)
{
    public static DeliveryReadModel From(Delivery delivery)
        => new(delivery.Operator.ToString(), delivery.TrackingNumber, delivery.Street, delivery.City, delivery.PostalCode);
}