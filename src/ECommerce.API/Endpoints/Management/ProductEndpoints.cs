using ECommerce.API.Endpoints.Management.Requests;
using ECommerce.ApplicationCore.Features.Management.Products;
using ECommerce.ApplicationCore.Features.Management.Products.Commands;
using ECommerce.ApplicationCore.Features.Management.Products.Queries;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Models;
using ECommerce.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints.Management;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder RegisterProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Products Management";
        
        endpoints.MapGet("api/management/products", GetPaginated)
            .Produces<List<ManagementProductReadModel>>()
            .WithTags(groupName);
            // .RequireAuthorization(AuthorizationPolicy.Admin);
        
        endpoints.MapGet("api/management/products/{id:long}", GetById)
            .Produces<ManagementProductReadModel>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(groupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);

        endpoints.MapPost("api/management/products", Create)
            .Produces<ManagementProductReadModel>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(groupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        endpoints.MapPut("api/management/products/{id:long}/update-details", UpdateDetails)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .WithTags(groupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        endpoints.MapPut("api/management/products/{id:long}/update-sale-data",  UpdateSaleData)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(groupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);

        return endpoints;
    }

    private static async ValueTask<IResult> GetPaginated(
        [AsParameters] GetPaginatedManagementProductsRequest request,
        [FromServices] IQueryHandler<GetPaginatedManagementProductsQuery, PaginatedList<ManagementProductReadModel>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetPaginatedManagementProductsQuery
        {
            PageSize = 1,
            PageNumber = 1,
            SearchPhrase = null,
            SortBy = null,
            SortDirection = null
        }, cancellationToken);
        return Results.Ok(result);
    }
    
    private static async ValueTask<IResult> GetById(
        [FromRoute] long id,
        [FromServices] IQueryHandler<GetProductByIdQuery, ManagementProductReadModel> handler, 
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(
            new GetProductByIdQuery
            {
                Id = id
            }, cancellationToken);
        return Results.Ok(result);
    }
    
    private static async ValueTask<IResult> Create(
        [FromBody] CreateProductRequest request,
        [FromServices] ICommandHandler<CreateProductCommand, long> handler,
        CancellationToken cancellationToken)
    {
        var id = await handler.HandleAsync(new CreateProductCommand
        {
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
            Price = request.Price,
            Quantity = request.Quantity,
            IsActive = request.IsActive
        }, cancellationToken);
        return Results.Created($"api/products/{id}", null);
    }
    
    private static async ValueTask<IResult> UpdateDetails(
        [FromRoute] long id,
        [FromBody] UpdateProductDetailsRequest request,
        [FromServices] ICommandHandler<UpdateProductDetailsCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new UpdateProductDetailsCommand
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            CategoryId = request.CategoryId,
        }, cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> UpdateSaleData(
        [FromRoute] long id,
        [FromBody] UpdateProductSaleDataRequest request, 
        [FromServices] ICommandHandler<UpdateProductSaleDataCommand> handler, 
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new UpdateProductSaleDataCommand
            {
                Id = id,
                Price = request.Price, 
                Quantity = request.Quantity,
                IsActive = request.IsActive
            }, cancellationToken);
        return Results.NoContent();
    }
}