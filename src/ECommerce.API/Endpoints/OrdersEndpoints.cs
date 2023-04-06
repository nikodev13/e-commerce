using ECommerce.API.Endpoints.Requests;
using ECommerce.ApplicationCore.Features.Orders.Commands;
using ECommerce.ApplicationCore.Features.Orders.Queries;
using ECommerce.ApplicationCore.Features.Orders.ReadModels;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Models;
using ECommerce.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder RegisterOrderEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Customer Orders";

        endpoints.MapGet("api/orders/{id:long}", GetOrderById)
            .Produces<OrderReadModel>()
            .WithTags(groupName);
        
        endpoints.MapGet("api/orders", GetPaginatedOrders)
            .Produces<PaginatedList<OrderReadModel>>()
            .WithTags(groupName);
        
        endpoints.MapPost("api/orders/{id:long}", GetOrderByIdForManagement)
            .Produces(StatusCodes.Status201Created)
            .WithTags(groupName);
        
        endpoints.MapPost("api/orders/place", PlaceOrder)
            .Produces(StatusCodes.Status201Created)
            .WithTags(groupName);
        
        endpoints.MapPut("api/orders/{id:long}", ChangeOrderLineQuantity)
            .RequireAuthorization(AuthorizationPolicy.Admin)
            .Produces(StatusCodes.Status204NoContent)
            .WithTags(groupName);
        
        endpoints.MapPatch("api/orders/{id:long}", SetOrderStatus)
            .RequireAuthorization(AuthorizationPolicy.Admin)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(groupName);

        return endpoints;
    }
    
    private static async ValueTask<IResult> GetOrderById(
        [FromRoute] long id,
        [FromServices] IQueryHandler<GetOrderByIdQuery, OrderReadModel> handler,
        CancellationToken cancellationToken)
    {
        var order = await handler.HandleAsync(new GetOrderByIdQuery(id), cancellationToken);
        return Results.Ok(order);
    }
    
    private static async ValueTask<IResult> GetPaginatedOrders(
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

    private static async ValueTask<IResult> GetOrderByIdForManagement(
        [FromBody] GetPaginatedOrdersOrdersInListForManagementRequest request,
        [FromServices] IQueryHandler<GetPaginatedOrdersInListForManagementQuery, PaginatedList<OrderInListReadModel>> handler,
        CancellationToken cancellationToken)
    {
        var paginatedOrdersInList = await handler.HandleAsync(request, cancellationToken);
        return Results.Ok(paginatedOrdersInList);
    }


    public static async ValueTask<IResult> ChangeOrderLineQuantity(
        [FromRoute] long id,
        [FromBody] ChangeOrderLineQuantityRequest request,
        [FromServices] ICommandHandler<ChangeOrderLineQuantityCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(request.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }
    
    public static async ValueTask<IResult> SetOrderStatus(
        [FromRoute] long id,
        [FromBody] SetOrderStatusRequestOrderStatus request,
        [FromServices] ICommandHandler<SetOrderStatusCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(request.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }
}