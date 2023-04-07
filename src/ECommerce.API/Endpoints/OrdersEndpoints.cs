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
        
        endpoints.MapPatch("api/orders/{id:long}/change-line", ChangeOrderLineQuantity)
            .RequireAuthorization(AuthorizationPolicy.Admin)
            .Produces(StatusCodes.Status204NoContent)
            .WithTags(groupName);
        
        endpoints.MapPatch("api/orders/{id:long}/change-status", SetOrderStatus)
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
        [AsParameters] GetPaginatedOrdersRequestParameters requestParameters,
        [FromServices] IQueryHandler<GetPaginatedOrdersQuery, PaginatedList<OrderReadModel>> handler,
        CancellationToken cancellationToken)
    {
        var orders = await handler.HandleAsync(requestParameters.ToQuery(), cancellationToken);
        return Results.Ok(orders);
    }
    
    private static async ValueTask<IResult> PlaceOrder(
        [FromBody] PlaceOrderRequestBody requestBody,
        [FromServices] ICommandHandler<PlaceOrderCommand, long> handler,
        CancellationToken cancellationToken)
    {
        var orderId = await handler.HandleAsync(requestBody.ToCommand(), cancellationToken);
        return Results.Created($"api/orders/{orderId}", null);
    }

    private static async ValueTask<IResult> GetOrderByIdForManagement(
        [FromBody] GetPaginatedOrdersForManagementRequestBody requestBody,
        [FromServices] IQueryHandler<GetPaginatedOrdersForManagementQuery, PaginatedList<OrderInListReadModel>> handler,
        CancellationToken cancellationToken)
    {
        var paginatedOrdersInList = await handler.HandleAsync(requestBody.ToQuery(), cancellationToken);
        return Results.Ok(paginatedOrdersInList);
    }


    public static async ValueTask<IResult> ChangeOrderLineQuantity(
        [FromRoute] long id,
        [FromBody] ChangeOrderLineQuantityRequestBody requestBody,
        [FromServices] ICommandHandler<ChangeOrderLineQuantityCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(requestBody.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }
    
    public static async ValueTask<IResult> SetOrderStatus(
        [FromRoute] long id,
        [FromBody] SetOrderStatusRequestBody request,
        [FromServices] ICommandHandler<SetOrderStatusCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(request.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }
}