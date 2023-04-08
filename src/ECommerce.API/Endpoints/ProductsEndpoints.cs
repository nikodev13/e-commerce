using ECommerce.API.Endpoints.RequestBodies;
using ECommerce.ApplicationCore.Features.Products.Commands;
using ECommerce.ApplicationCore.Features.Products.Queries;
using ECommerce.ApplicationCore.Features.Products.ReadModels;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Models;
using ECommerce.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        
        endpoints.MapGet("api/products/{id}/quantity", GetProductQuantity)
            .WithTags(groupName);

        const string managementGroupName = "Products Management";
        endpoints.MapGet("api/products/{id:long}/history", GetProductHistoryData)
            .Produces<ProductHistoryReadModel>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(managementGroupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        endpoints.MapPost("api/products", Create)
            .Produces<ProductReadModel>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(managementGroupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        endpoints.MapPatch("api/products/{id:long}/update-details", UpdateDetails)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .WithTags(managementGroupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        endpoints.MapPatch("api/products/{id:long}/update-sale-data", UpdateSaleData)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(managementGroupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);

        endpoints.MapGet("api/products/{productId:long}/image", GetProductImage)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(groupName);
        endpoints.MapPut("api/products/{productId:long}/image", SetProductImage)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(managementGroupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        
        return endpoints;
    }

    private static async ValueTask<IResult> GetPaginated(
        [AsParameters] GetPaginatedCustomerProductsRequestParameters requestParameters,
        [FromServices] IQueryHandler<GetPaginatedCustomerProductsQuery, PaginatedList<ProductReadModel>> handler,
        CancellationToken cancellationToken)
    {
        var paginatedList = await handler.HandleAsync(requestParameters, cancellationToken);
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

    private static async ValueTask<IResult> GetProductQuantity(
        [FromRoute] long id,
        [FromServices] IQueryHandler<GetProductQuantityQuery, ProductQuantityReadModel> handler,
        CancellationToken cancellationToken)
    {
        var readModel = await handler.HandleAsync(new GetProductQuantityQuery(id), cancellationToken);
        return Results.Ok(readModel);
    }
    
    private static async ValueTask<IResult> GetProductHistoryData(
        [FromRoute] long id,
        [FromServices] IQueryHandler<GetProductHistoryDataQuery, ProductHistoryReadModel> handler,
        CancellationToken cancellationToken)
    {
        var readModel = await handler.HandleAsync(new GetProductHistoryDataQuery(id), cancellationToken);
        return Results.Ok(readModel);
    }
    
    private static async ValueTask<IResult> Create(
        [FromBody] CreateProductRequestBody requestBody,
        [FromServices] ICommandHandler<CreateProductCommand, long> handler,
        CancellationToken cancellationToken)
    {
        var id = await handler.HandleAsync(requestBody.ToCommand(), cancellationToken);
        return Results.Created($"api/products/{id}", null);
    }
    
    private static async ValueTask<IResult> UpdateDetails(
        [FromRoute] long id,
        [FromBody] UpdateProductDetailsRequestBody requestBody,
        [FromServices] ICommandHandler<UpdateProductDetailsCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(requestBody.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }

    private static async ValueTask<IResult> UpdateSaleData(
        [FromRoute] long id,
        [FromBody] UpdateProductSaleDataRequestBody requestBody,
        [FromServices] ICommandHandler<UpdateProductSaleDataCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(requestBody.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }

    private static IResult GetProductImage([FromRoute] long productId)
    {
        var rootPath = Directory.GetCurrentDirectory();

        var imagePath = $"{rootPath}/wwwroot/ProductImages/{productId}.jpg";

        var imageExists = File.Exists(imagePath);
        if (!imageExists) return Results.NotFound($"Product image with id `{productId}` does not exists");

        using var fileStream = new FileStream(imagePath, FileMode.Open);
        
        return Results.File(fileStream, "image/jpeg");
    }

    // TODO
    private static async ValueTask<IResult> SetProductImage(
        [FromRoute] long productId,
        [FromForm] IFormFile? image,
        [FromServices] IAppDbContext dbContext,
        CancellationToken cancellationToken)
    {
        if (!await dbContext.Products.AnyAsync(x => x.Id == productId, cancellationToken)) 
            return Results.BadRequest($"Product with id `{productId}` does not exist.");

        if (image is not { Length: > 0, ContentType: "image/jpeg" })
            return Results.BadRequest("The image must be JPEG file type.");
        
        var rootPath = Directory.GetCurrentDirectory();
        var imagePath = $"{rootPath}/wwwroot/ProductImages/{productId}.jpg";

        await using var fileStream = new FileStream(imagePath, FileMode.Create);
        await image.CopyToAsync(fileStream, cancellationToken);
            
        return Results.NoContent();
    }
}