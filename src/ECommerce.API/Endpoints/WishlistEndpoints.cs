using ECommerce.API.Endpoints.RequestBodies;
using ECommerce.ApplicationCore.Features.Wishlist.Commands;
using ECommerce.ApplicationCore.Features.Wishlist.Queries;
using ECommerce.ApplicationCore.Features.Wishlist.ReadModels;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

public static class WishlistEndpoints
{
    public static IEndpointRouteBuilder RegisterWishlistEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Customer product wishlist";

        endpoints.MapGet("api/wishlist", GetAllProductsFromWishList)
            .Produces<List<WishlistProductReadModel>>()
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithGroupName(groupName);
        endpoints.MapPost("api/wishlist", AddProductToWishlist)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithGroupName(groupName);
        endpoints.MapDelete("api/wishlist", RemoveProductFromWishlist)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithGroupName(groupName);
        
        return endpoints;
    }

    private static async Task<IResult> GetAllProductsFromWishList(
        [FromServices] IQueryHandler<GetAllProductsFromWishlistQuery, List<WishlistProductReadModel>> handler,
        CancellationToken cancellationToken)
    {
        var wishlist = await handler.HandleAsync(new GetAllProductsFromWishlistQuery(), cancellationToken);
        return Results.Ok(wishlist);
    }
    
    private static async Task<IResult> AddProductToWishlist(
        [FromBody] AddProductToWishlistRequestBody requestBody,
        [FromServices] ICommandHandler<AddProductToWishlistCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(requestBody, cancellationToken);
        return Results.NoContent();
    }
    
    private static async Task<IResult> RemoveProductFromWishlist(
        [FromBody] RemoveProductFromWishlistRequestBody requestBody,
        [FromServices] ICommandHandler<RemoveProductFromWishlistCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(requestBody, cancellationToken);
        return Results.NoContent();
    }
}