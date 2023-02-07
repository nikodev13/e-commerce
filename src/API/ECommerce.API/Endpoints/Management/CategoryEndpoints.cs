using ECommerce.API.Endpoints.Management.Requests;
using ECommerce.Application.Management.Categories;
using ECommerce.Application.Management.Categories.Commands;
using ECommerce.Application.Management.Categories.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuthorizationPolicy = ECommerce.Infrastructure.Authorization.AuthorizationPolicy;

namespace ECommerce.API.Endpoints.Management
{
    public static class CategoryEndpoints
    {
        public static IEndpointRouteBuilder RegisterCategoryEndpoints(this IEndpointRouteBuilder endpoints)
        {
            const string groupName = "Categories Management";
            
            endpoints.MapGet("api/management/categories", async (IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetAllCategoriesQuery());
                    return Results.Ok(result);
                })
                .Produces<List<CategoryReadModel>>()
                .WithTags(groupName)
                .RequireAuthorization(AuthorizationPolicy.Admin);

            endpoints.MapGet("api/management/categories/{id:long}", async (IMediator mediator, [FromRoute] long id) =>
                {
                    var result = await mediator.Send(new GetCategoryByIdQuery { Id = id });
                    return Results.Ok(result);
                })
                .Produces<CategoryReadModel>()
                .Produces(StatusCodes.Status404NotFound)
                .WithTags(groupName)
                .RequireAuthorization(AuthorizationPolicy.Admin);

            endpoints.MapPost("api/management/categories", async (IMediator mediator, [FromBody] CreateCategoryRequest request) =>
                {
                    var result = await mediator.Send(new CreateCategoryCommand { Name = request.Name });
                    return Results.Created($"api/products/categories/{result.Id}", result);
                })
                .Produces<CategoryReadModel>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status409Conflict)
                .WithTags(groupName)
                .RequireAuthorization(AuthorizationPolicy.Admin);

            endpoints.MapPut("api/management/categories/{id:long}", async (IMediator mediator, [FromRoute] long id, [FromBody] UpdateCategoryRequest request) =>
                {
                    await mediator.Send(new UpdateCategoryCommand { Id = id, Name = request.Name});
                    return Results.NoContent();
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags(groupName)
                .RequireAuthorization(AuthorizationPolicy.Admin);

            endpoints.MapDelete("api/management/categories/{id:long}", async (IMediator mediator, [FromRoute] long id) =>
                {
                    await mediator.Send(new DeleteCategoryCommand { Id = id });
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
