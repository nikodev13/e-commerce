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

public record ChangeOrderLineQuantityRequest(long OrderId, long ProductId, uint NewQuantity)
    : ChangeOrderLineQuantityCommand(OrderId, ProductId, NewQuantity)
{
    public ChangeOrderLineQuantityCommand ToCommand() => this;
}