using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.Orders.ReadModels;

public record DeliveryAddressReadModel(string Street, string City, string PostalCode)
{
    public static DeliveryAddressReadModel From(DeliveryAddress address)
        => new(address.Street, address.City, address.PostalCode);
}