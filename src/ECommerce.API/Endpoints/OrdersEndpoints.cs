using ECommerce.API.Endpoints.RequestBodies;
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
        
        endpoints.MapPatch("api/orders/{id:long}/change-line-quantity", ChangeOrderLineQuantity)
            .RequireAuthorization(AuthorizationPolicy.Admin)
            .Produces(StatusCodes.Status204NoContent)
            .WithTags(groupName);
        
        endpoints.MapPatch("api/orders/{id:long}/change-status", SetOrderStatus)
            .RequireAuthorization(AuthorizationPolicy.Admin)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(groupName);
        
        endpoints.MapPatch("api/orders/{id:long}/set-delivery-tracking-number", SetDeliveryTrackingNumber)
            .RequireAuthorization(AuthorizationPolicy.Admin)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(groupName);
        
        endpoints.MapPatch("api/orders/{id:long}/change-delivery-address", ChangeDeliveryAddress)
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
        [AsParameters] GetPaginatedOrdersRequestParameters parameters,
        [FromServices] IQueryHandler<GetPaginatedOrdersQuery, PaginatedList<OrderReadModel>> handler,
        CancellationToken cancellationToken)
    {
        var orders = await handler.HandleAsync(parameters.ToQuery(), cancellationToken);
        return Results.Ok(orders);
    }
    
    private static async ValueTask<IResult> PlaceOrder(
        [FromBody] PlaceOrderRequestBody body,
        [FromServices] ICommandHandler<PlaceOrderCommand, long> handler,
        CancellationToken cancellationToken)
    {
        var orderId = await handler.HandleAsync(body.ToCommand(), cancellationToken);
        return Results.Created($"api/orders/{orderId}", null);
    }

    private static async ValueTask<IResult> GetOrderByIdForManagement(
        [FromBody] GetPaginatedOrdersForManagementRequestBody body,
        [FromServices] IQueryHandler<GetPaginatedOrdersForManagementQuery, PaginatedList<OrderInListReadModel>> handler,
        CancellationToken cancellationToken)
    {
        var paginatedOrdersInList = await handler.HandleAsync(body.ToQuery(), cancellationToken);
        return Results.Ok(paginatedOrdersInList);
    }


    private static async ValueTask<IResult> ChangeOrderLineQuantity(
        [FromRoute] long id,
        [FromBody] ChangeOrderLineQuantityRequestBody body,
        [FromServices] ICommandHandler<ChangeOrderLineQuantityCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(body.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> SetOrderStatus(
        [FromRoute] long id,
        [FromBody] SetOrderStatusRequestBody body,
        [FromServices] ICommandHandler<SetOrderStatusCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(body.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> SetDeliveryTrackingNumber(
        [FromRoute] long id,
        [FromBody] SetDeliverTrackingNumberRequestBody body,
        [FromServices] ICommandHandler<SetDeliveryTrackingNumberCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(body.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> ChangeDeliveryAddress(
        [FromRoute] long id,
        [FromBody] ChangeOrderDeliveryAddressRequestBody body,
        [FromServices] ICommandHandler<ChangeOrderDeliveryAddressCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(body.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }
}