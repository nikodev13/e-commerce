using ECommerce.API.Endpoints.RequestBodies;
using ECommerce.ApplicationCore.Features.Categories.Commands;
using ECommerce.ApplicationCore.Features.Categories.Queries;
using ECommerce.ApplicationCore.Features.Categories.ReadModels;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

public static class CategoriesEndpoints
{
    public static IEndpointRouteBuilder RegisterCategoriesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Categories"; 
        
        endpoints.MapGet("api/categories", GetAllCustomerCategories)
            .WithTags(groupName);

        const string managementGroupName = "Categories Management";

        endpoints.MapGet("api/categories/{id:long}", GetCategoryHistoryData)
            .Produces<CategoryHistoryReadModel>()
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(managementGroupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);

        endpoints.MapPost("api/categories", Create)
            .Produces<CategoryReadModel>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status409Conflict)
            .WithTags(managementGroupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);

        endpoints.MapPut("api/categories/{id:long}", Update)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(managementGroupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);

        endpoints.MapDelete("api/categories/{id:long}", Delete)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags(managementGroupName)
            .RequireAuthorization(AuthorizationPolicy.Admin);
        
        return endpoints;
    }

    private static async ValueTask<IResult> GetAllCustomerCategories(
        [FromServices] IQueryHandler<GetAllCategoriesQuery, List<CategoryReadModel>> handler,
        CancellationToken cancellationToken)

    {
        var readModels = await handler.HandleAsync(new GetAllCategoriesQuery(), cancellationToken);
        return Results.Ok(readModels);
    }
    
    private static async ValueTask<IResult> GetCategoryHistoryData(
        [FromRoute] long id,
        [FromServices] IQueryHandler<GetCategoryHistoryDataQuery, CategoryHistoryReadModel> handler,
        CancellationToken cancellationToken)

    {
        var readModels = await handler.HandleAsync(new GetCategoryHistoryDataQuery(id), cancellationToken);
        return Results.Ok(readModels);
    }
    
    private static async ValueTask<IResult> Create(
        [FromBody] CreateCategoryRequestBody body, 
        [FromServices] ICommandHandler<CreateCategoryCommand, long> handler,
        CancellationToken cancellationToken)
    {
        var id = await handler.HandleAsync(body.ToCommand(), cancellationToken);
        return Results.Created($"api/products/categories/{id}", null);
    }

    private static async ValueTask<IResult> Update(
        [FromRoute] long id,
        [FromBody] UpdateCategoryRequestBody body,
        [FromServices] ICommandHandler<UpdateCategoryCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(body.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }

    private static async ValueTask<IResult> Delete(
        [FromRoute] long id,
        [FromServices] ICommandHandler<DeleteCategoryCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new DeleteCategoryCommand(id), cancellationToken);
        return Results.NoContent();
    }
}