using ECommerce.API.Endpoints.Requests;
using ECommerce.ApplicationCore.Features.Products.Commands;
using ECommerce.ApplicationCore.Features.Products.Queries;
using ECommerce.ApplicationCore.Features.Products.ReadModels;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Models;
using ECommerce.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

public static class ProductsEndpoints
{
    public static IEndpointRouteBuilder RegisterProductsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Products";
        
        endpoints.MapGet("api/products", GetPaginated)
            .Produces<PaginatedList<ProductReadModel>>()
            .WithTags(groupName);
        
        endpoints.MapGet("api/products/{id}", GetById)
            .WithTags(groupName);
        
        endpoints.MapGet("api/products/{id}/quantity", GetProductQuantityById)
            .WithTags(groupName);

        const string managementGroupName = "Products Management";
        endpoints.MapPost("api/products", Create)
            .Produces<ProductReadModel>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(managementGroupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        endpoints.MapPut("api/products/{id:long}/update-details", UpdateDetails)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .WithTags(managementGroupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        endpoints.MapPut("api/products/{id:long}/update-sale-data",  UpdateSaleData)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(managementGroupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);

        
        return endpoints;
    }

    private static async ValueTask<IResult> GetPaginated(
        [AsParameters] GetPaginatedCustomerProductsRequest request,
        [FromServices] IQueryHandler<GetPaginatedCustomerProductsQuery, PaginatedList<ProductReadModel>> handler,
        CancellationToken cancellationToken)
    {
        var paginatedList = await handler.HandleAsync(request, cancellationToken);
        return Results.Ok(paginatedList);
    }

    private static async ValueTask<IResult> GetById(
        [FromRoute] long id,
        [FromServices] IQueryHandler<GetProductByIdQuery, ProductReadModel> handler,
        CancellationToken cancellationToken)
    {
        var readModel = await handler.HandleAsync(new GetProductByIdQuery(id), cancellationToken) ;
        return Results.Ok(readModel);
    }

    private static async ValueTask<IResult> GetProductQuantityById(
        [FromRoute] long id,
        [FromServices] IQueryHandler<GetProductQuantityByIdQuery, ProductQuantityReadModel> handler,
        CancellationToken cancellationToken)
    {
        var readModel = await handler.HandleAsync(new GetProductQuantityByIdQuery(id), cancellationToken);
        return Results.Ok(readModel);
    }
    
    private static async ValueTask<IResult> Create(
        [FromBody] CreateProductRequest request,
        [FromServices] ICommandHandler<CreateProductCommand, long> handler,
        CancellationToken cancellationToken)
    {
        var id = await handler.HandleAsync(request.ToCommand(), cancellationToken);
        return Results.Created($"api/products/{id}", null);
    }
    
    private static async ValueTask<IResult> UpdateDetails(
        [FromRoute] long id,
        [FromBody] UpdateProductDetailsRequest request,
        [FromServices] ICommandHandler<UpdateProductDetailsCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(request.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> UpdateSaleData(
        [FromRoute] long id,
        [FromBody] UpdateProductSaleDataRequest request, 
        [FromServices] ICommandHandler<UpdateProductSaleDataCommand> handler, 
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(request.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }
}