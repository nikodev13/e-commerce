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
            .WithTags(groupName);
        endpoints.MapPost("api/wishlist", AddProductToWishlist)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(groupName);
        endpoints.MapDelete("api/wishlist", RemoveProductFromWishlist)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(groupName);
        
        return endpoints;
    }

    private static async ValueTask<IResult> GetAllProductsFromWishList(
        [FromServices] IQueryHandler<GetAllProductsFromWishlistQuery, List<WishlistProductReadModel>> handler,
        CancellationToken cancellationToken)
    {
        var wishlist = await handler.HandleAsync(new GetAllProductsFromWishlistQuery(), cancellationToken);
        return Results.Ok(wishlist);
    }
    
    private static async ValueTask<IResult> AddProductToWishlist(
        [FromBody] AddProductToWishlistRequestBody body,
        [FromServices] ICommandHandler<AddProductToWishlistCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(body, cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> RemoveProductFromWishlist(
        [FromBody] RemoveProductFromWishlistRequestBody body,
        [FromServices] ICommandHandler<RemoveProductFromWishlistCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(body, cancellationToken);
        return Results.NoContent();
    }
}