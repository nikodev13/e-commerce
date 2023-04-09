using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Orders.Commands;
using ECommerce.ApplicationCore.Features.Orders.Queries;

namespace ECommerce.API.Endpoints.RequestBodies;

public record GetPaginatedOrdersRequestParameters(int PageSize, int PageNumber, OrderStatus? OrderStatus)
    : GetPaginatedOrdersQuery(PageSize, PageNumber, OrderStatus)
{
    public GetPaginatedOrdersQuery ToQuery() => this;
}

public record PlaceOrderRequestBody(List<PlaceOrderCommand.OrderLine> OrderLines, PlaceOrderCommand.DeliveryOptions DeliveryOptionsDeliveryOptions)
    : PlaceOrderCommand(OrderLines, DeliveryOptionsDeliveryOptions)
{
    public PlaceOrderCommand ToCommand() => this;
}

public record GetPaginatedOrdersForManagementRequestParameters(int PageSize, int PageNumber, OrderStatus OrderStatus = OrderStatus.Paid)
    : GetPaginatedOrdersForManagementQuery(PageSize, PageNumber, OrderStatus)
{
    public GetPaginatedOrdersForManagementQuery ToQuery() => this;
}

public record ChangeOrderLineQuantityRequestBody(long ProductId, uint NewQuantity)
{
    public ChangeOrderLineQuantityCommand ToCommand(long orderId) => new(orderId, ProductId, NewQuantity);
}

public record SetOrderStatusRequestBody(OrderStatus OrderStatus)
{
    public SetOrderStatusCommand ToCommand(long orderId) => new(orderId, OrderStatus);
}

public record ChangeOrderDeliveryAddressRequestBody(string Street, string PostalCode, string City)
{
    public ChangeOrderDeliveryAddressCommand ToCommand(long orderId) => new(orderId, Street, PostalCode, City);
}

public record SetDeliverTrackingNumberRequestBody(string TrackingNumber)
{
    public SetDeliveryTrackingNumberCommand ToCommand(long orderId) => new(orderId, TrackingNumber);
}