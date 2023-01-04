using ECommerce.API.Requests;
using ECommerce.Application.Categories.Commands;
using ECommerce.Application.Categories.Queries;
using ECommerce.Application.Categories.ReadModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuthorizationPolicy = ECommerce.Infrastructure.Authorization.AuthorizationPolicy;

namespace ECommerce.API.Endpoints
{
    public static class CategoryEndpoints
    {
        private static async Task<IResult> GetAll([FromServices] IMediator mediator)
        {
            var result = await mediator.Send(new GetAllCategoriesQuery());
            return Results.Ok(result);
        }
        
        private static async Task<IResult> GetById([FromServices] IMediator mediator, [FromRoute] long id)
        {
            var result = await mediator.Send(new GetCategoryByIdQuery(id));
            return Results.Ok(result);
        }

        private static async Task<IResult> GetByName([FromServices] IMediator mediator, [FromRoute] string name)
        {
            var result = await mediator.Send(new GetCategoryByNameQuery(name));
            return Results.Ok(result);
        }

        private static async Task<IResult> Create([FromServices] IMediator mediator, [FromBody] CategoryNameRequest nameRequest)
        {
            var result = await mediator.Send(new CreateCategoryCommand(nameRequest.CategoryName));
            return Results.Created($"api/products/categories/{result.Id}", result);
        }

        private static async Task<IResult> Update([FromServices] IMediator mediator, [FromRoute] long id, [FromBody] CategoryNameRequest nameRequest)
        {
            await mediator.Send(new UpdateCategoryCommand(id, nameRequest.CategoryName));
            return Results.NoContent();
        }

        private static async Task<IResult> Delete([FromServices] IMediator mediator, [FromRoute] long id)
        {
            await mediator.Send(new DeleteCategoryCommand(id));
            return Results.NoContent();
        }

        public static void RegisterCategoryEndpoints(this WebApplication app)
        {
            var groupName = "Product categories";
            app.MapGet("api/products/categories", GetAll)
                .WithTags(groupName)
                .Produces<List<CategoryReadModel>>()
                .AllowAnonymous();

            app.MapGet("api/products/categories/{id:long}", GetById)
                .WithTags(groupName)
                .Produces<CategoryReadModel>()
                .Produces(StatusCodes.Status404NotFound)
                .AllowAnonymous();

            app.MapGet("api/products/categories/{name}", GetByName)
                .WithTags(groupName)
                .Produces<CategoryReadModel>()
                .Produces(StatusCodes.Status404NotFound)
                .AllowAnonymous();

            app.MapPost("api/products/categories", Create)
                .WithTags(groupName)
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status409Conflict)
                .RequireAuthorization(AuthorizationPolicy.Admin);

            app.MapPut("api/products/categories/{id:long}", Update)
                .WithTags(groupName)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .RequireAuthorization(AuthorizationPolicy.Admin);

            app.MapDelete("api/products/categories/{id:long}", Delete)
                .WithTags(groupName)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .RequireAuthorization(AuthorizationPolicy.Admin);

        }
    }
}
