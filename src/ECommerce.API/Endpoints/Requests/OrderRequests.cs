
using ECommerce.ApplicationCore.Features.Orders.Commands;

namespace ECommerce.API.Endpoints.Requests;

public class PlaceOrderRequest
{
    public required List<PlaceOrderCommand.OrderLine> OrderLines { get; init; }
    public required PlaceOrderCommand.Address DeliveryAddress { get; init; }
}

