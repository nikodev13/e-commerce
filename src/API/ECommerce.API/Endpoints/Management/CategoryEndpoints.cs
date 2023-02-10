using ECommerce.API.Endpoints.Management.Requests;
using ECommerce.Application.Management.Categories;
using ECommerce.Application.Management.Categories.Commands;
using ECommerce.Application.Management.Categories.Queries;
using ECommerce.Application.Shared.CQRS;
using Microsoft.AspNetCore.Mvc;
using AuthorizationPolicy = ECommerce.Infrastructure.Authorization.AuthorizationPolicy;

namespace ECommerce.API.Endpoints.Management
{
    public static class CategoryEndpoints
    {
        public static IEndpointRouteBuilder RegisterCategoryEndpoints(this IEndpointRouteBuilder endpoints)
        {
            const string groupName = "Categories Management";
            
            endpoints.MapGet("api/management/categories", 
                    async ([FromServices] IQueryDispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    var result = await dispatcher.Dispatch(new GetAllCategoriesQuery(), cancellationToken);
                    return Results.Ok(result);
                })
                .Produces<List<CategoryReadModel>>()
                .WithTags(groupName)
                .RequireAuthorization(AuthorizationPolicy.Admin);

            endpoints.MapGet("api/management/categories/{id:long}", 
                    async ([FromRoute] long id, [FromServices] IQueryDispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    var result = await dispatcher.Dispatch(new GetCategoryByIdQuery { Id = id }, cancellationToken);
                    return Results.Ok(result);
                })
                .Produces<CategoryReadModel>()
                .Produces(StatusCodes.Status404NotFound)
                .WithTags(groupName)
                .RequireAuthorization(AuthorizationPolicy.Admin);

            endpoints.MapPost("api/management/categories", 
                    async ([FromBody] CreateCategoryRequest request, [FromServices] ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    var id = await dispatcher.Dispatch(new CreateCategoryCommand { Name = request.Name }, cancellationToken);
                    return Results.Created($"api/products/categories/{id}", null);
                })
                .Produces<CategoryReadModel>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status409Conflict)
                .WithTags(groupName)
                .RequireAuthorization(AuthorizationPolicy.Admin);

            endpoints.MapPut("api/management/categories/{id:long}",
                    async ([FromRoute] long id, [FromBody] UpdateCategoryRequest request, [FromServices] ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    await dispatcher.Dispatch(new UpdateCategoryCommand { Id = id, Name = request.Name }, cancellationToken);
                    return Results.NoContent();
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags(groupName)
                .RequireAuthorization(AuthorizationPolicy.Admin);

            endpoints.MapDelete("api/management/categories/{id:long}",
                    async ([FromRoute] long id, [FromServices] ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
                {
                    await dispatcher.Dispatch(new DeleteCategoryCommand { Id = id }, cancellationToken);
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
}
