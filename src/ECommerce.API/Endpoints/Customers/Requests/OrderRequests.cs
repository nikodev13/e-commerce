
using ECommerce.ApplicationCore.Features.Customers.Orders.Commands;

namespace ECommerce.API.Endpoints.Customers.Requests;

public class PlaceOrderRequest
{
    public required List<PlaceOrderCommand.OrderLine> OrderLines { get; init; }
    public required PlaceOrderCommand.Address DeliveryAddress { get; init; }
}

