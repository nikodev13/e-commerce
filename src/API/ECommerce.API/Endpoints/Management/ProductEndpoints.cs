using ECommerce.API.Endpoints.Management.Requests;
using ECommerce.Application.Management.Products;
using ECommerce.Application.Management.Products.Commands;
using ECommerce.Application.Management.Products.Queries;
using ECommerce.Application.Shared.CQRS;
using ECommerce.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints.Management;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder RegisterProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Products Management";
        
        endpoints.MapGet("api/management/products", 
                async ([FromServices] IQueryDispatcher dispatcher, CancellationToken cancellationToken) =>
            {
                var result = await dispatcher.DispatchAsync<GetAllProductsQuery, List<ProductReadModel>>(new GetAllProductsQuery(), cancellationToken);
                return Results.Ok(result);
            })
            .Produces<List<ProductReadModel>>()
            .WithTags(groupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        endpoints.MapGet("api/management/products/{id:long}",
                async ([FromRoute] long id, [FromServices] IQueryDispatcher dispatcher, CancellationToken cancellationToken) =>
            {
                var result = await dispatcher.DispatchAsync<GetProductByIdQuery, ProductReadModel>(new GetProductByIdQuery { Id = id }, cancellationToken);
                return Results.Ok(result);
            })
            .Produces<ProductReadModel>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(groupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);

        endpoints.MapPost("api/management/products", 
                async ([FromBody] CreateProductRequest request, [FromServices] ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
            {
                var id = await dispatcher.DispatchAsync<CreateProductCommand, long>(new CreateProductCommand
                {
                    Name = request.Name,
                    Description = request.Description,
                    CategoryId = request.CategoryId,
                    Price = request.Price,
                    Quantity = request.Quantity,
                    IsActive = request.IsActive
                }, cancellationToken);
                return Results.Created($"api/management/products/{id}", null);
            })
            .Produces<ProductReadModel>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(groupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        endpoints.MapPut("api/management/products/{id:long}/update-details", 
                async ([FromRoute] long id, [FromBody] UpdateProductDetailsRequest request, [FromServices] ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
            {
                await dispatcher.DispatchAsync(new UpdateProductDetailsCommand
                {
                    Id = id,
                    Name = request.Name,
                    Description = request.Description,
                    CategoryId = request.CategoryId,
                }, cancellationToken);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .WithTags(groupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        endpoints.MapPut("api/management/products/{id:long}/update-sale-data", 
                async ([FromRoute] long id, [FromBody] UpdateProductSaleDataRequest request, [FromServices] ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
            {
                await dispatcher.DispatchAsync(new UpdateProductSaleDataCommand
                {
                    Id = id,
                    Price = request.Price,
                    Quantity = request.Quantity,
                    IsActive = request.IsActive
                }, cancellationToken);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(groupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);

        return endpoints;
    }
}