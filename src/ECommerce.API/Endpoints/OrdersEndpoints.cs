using System.Runtime.CompilerServices;
using ECommerce.API.Endpoints.Requests;
using ECommerce.ApplicationCore.Features.Orders.Commands;
using ECommerce.ApplicationCore.Features.Orders.Queries;
using ECommerce.ApplicationCore.Features.Orders.ReadModels;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder RegisterOrderEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Customer Orders";
        
        endpoints.MapGet("api/orders/{id:long}", GetById);
        endpoints.MapGet("api/orders", GetPaginated);
        endpoints.MapPost("api/orders/place", PlaceOrder);
        endpoints.MapPut("api/orders/{id:long}", ChangeOrderLineQuantity);
        
        return endpoints;
    }
    
    private static async ValueTask<IResult> GetById(
        [FromRoute] long id,
        [FromServices] IQueryHandler<GetOrderByIdQuery, OrderReadModel> handler,
        CancellationToken cancellationToken)
    {
        var order = await handler.HandleAsync(new GetOrderByIdQuery(id), cancellationToken);
        return Results.Ok(order);
    }
    
    private static async ValueTask<IResult> GetPaginated(
        [AsParameters] GetPaginatedOrdersRequest request,
        [FromServices] IQueryHandler<GetPaginatedOrdersQuery, PaginatedList<OrderReadModel>> handler,
        CancellationToken cancellationToken)
    {
        var orders = await handler.HandleAsync(request.ToQuery(), cancellationToken);
        return Results.Ok(orders);
    }
    
    private static async ValueTask<IResult> PlaceOrder(
        [FromBody] PlaceOrderRequest request,
        [FromServices] ICommandHandler<PlaceOrderCommand, long> handler,
        CancellationToken cancellationToken)
    {
        var orderId = await handler.HandleAsync(request.ToCommand(), cancellationToken);
        return Results.Created($"api/orders/{orderId}", null);
    }

    public static async ValueTask<IResult> ChangeOrderLineQuantity(
        [FromRoute] long id,
        [FromBody] ChangeOrderLineQuantityRequest request,
        [FromServices] ICommandHandler<ChangeOrderLineQuantityCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(request.ToCommand(), cancellationToken);
        return Results.NoContent();
    }
    
}