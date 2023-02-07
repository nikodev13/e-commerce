using ECommerce.API.Endpoints.Management.Requests;
using ECommerce.Application.Management.Products;
using ECommerce.Application.Management.Products.Commands;
using ECommerce.Application.Management.Products.Queries;
using ECommerce.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints.Management;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder RegisterProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Products Management";
        
        endpoints.MapGet("api/management/products", 
                async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetAllProductsQuery());
                return Results.Ok(result);
            })
            .Produces<List<ProductReadModel>>()
            .WithTags(groupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        endpoints.MapGet("api/management/products/{id:long}",
                async (IMediator mediator, [FromRoute] long id) =>
            {
                var result = await mediator.Send(new GetProductByIdQuery { Id = id });
                return Results.Ok(result);
            })
            .Produces<ProductReadModel>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(groupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);

        endpoints.MapPost("api/management/products", 
                async (IMediator mediator, [FromBody] CreateProductRequest request) =>
            {
                var result = await mediator.Send(new CreateProductCommand
                {
                    Name = request.Name,
                    Description = request.Description,
                    CategoryId = request.CategoryId,
                    Price = request.Price,
                    Quantity = request.Quantity,
                    IsActive = request.IsActive
                });
                return Results.Created($"api/management/products/{result.Id}", result);
            })
            .Produces<ProductReadModel>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(groupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        endpoints.MapPut("api/management/products/{id:long}/update-details", 
                async (IMediator mediator, [FromRoute] long id, [FromBody] UpdateProductDetailsCommand request) =>
            {
                await mediator.Send(new UpdateProductDetailsCommand
                {
                    Id = id,
                    Name = request.Name,
                    Description = request.Description,
                    CategoryId = request.CategoryId,
                });
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .WithTags(groupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        endpoints.MapPut("api/management/products/{id:long}/update-sale-data", 
                async (IMediator mediator, [FromRoute] long id, [FromBody] UpdateProductSaleDataRequest request) =>
            {
                await mediator.Send(new UpdateProductSaleDataCommand
                {
                    Id = id,
                    Price = request.Price,
                    Quantity = request.Quantity,
                    IsActive = request.IsActive
                });
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