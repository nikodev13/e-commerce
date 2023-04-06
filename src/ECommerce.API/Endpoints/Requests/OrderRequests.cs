using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Orders.Commands;
using ECommerce.ApplicationCore.Features.Orders.Queries;

namespace ECommerce.API.Endpoints.Requests;

public record GetPaginatedOrdersRequest(int PageSize, int PageNumber, OrderStatus? OrderStatus)
    : GetPaginatedOrdersQuery(PageSize, PageNumber, OrderStatus)
{
    public GetPaginatedOrdersQuery ToQuery() => this;
}

public record PlaceOrderRequest(List<PlaceOrderCommand.OrderLine> OrderLines, PlaceOrderCommand.Address DeliveryAddress)
    : PlaceOrderCommand(OrderLines, DeliveryAddress)
{
    public PlaceOrderCommand ToCommand() => this;
}

public record GetPaginatedOrdersOrdersInListForManagementRequest(int PageSize, int PageNumber, OrderStatus OrderStatus = OrderStatus.Paid)
    : GetPaginatedOrdersInListForManagementQuery(PageSize, PageNumber, OrderStatus)
{
    public GetPaginatedOrdersInListForManagementQuery ToQuery() => this;
}

public record ChangeOrderLineQuantityRequest(long ProductId, uint NewQuantity)
{
    public ChangeOrderLineQuantityCommand ToCommand(long orderId) => new(orderId, ProductId, NewQuantity);
}

public record SetOrderStatusRequestOrderStatus(OrderStatus OrderStatus)
{
    public SetOrderStatusCommand ToCommand(long orderId) => new(orderId, OrderStatus);
}