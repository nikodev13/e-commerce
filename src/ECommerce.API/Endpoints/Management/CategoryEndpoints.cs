using ECommerce.API.Requests;
using ECommerce.ApplicationCore.Features.Management.Categories;
using ECommerce.ApplicationCore.Features.Management.Categories.Commands;
using ECommerce.ApplicationCore.Features.Management.Categories.Queries;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.AspNetCore.Mvc;
using AuthorizationPolicy = ECommerce.Infrastructure.Authorization.AuthorizationPolicy;

namespace ECommerce.API.Endpoints.Management
{
    public static class CategoryEndpoints
    {
        public static IEndpointRouteBuilder RegisterCategoryEndpoints(this IEndpointRouteBuilder endpoints)
        {
            const string groupName = "Categories Management";

            endpoints.MapGet("api/management/categories", GetAll)
                .Produces<List<CategoryReadModel>>()
                .WithTags(groupName)  
                .RequireAuthorization(AuthorizationPolicy.Admin);

            endpoints.MapGet("api/management/categories/{id:long}", GetById)
                .Produces<CategoryReadModel>()
                .Produces(StatusCodes.Status404NotFound)
                .WithTags(groupName)
                .RequireAuthorization(AuthorizationPolicy.Admin);

            endpoints.MapPost("api/management/categories", Create)
                .Produces<CategoryReadModel>(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status409Conflict)
                .WithTags(groupName)
                .RequireAuthorization(AuthorizationPolicy.Admin);

            endpoints.MapPut("api/management/categories/{id:long}", Update)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags(groupName)
                .RequireAuthorization(AuthorizationPolicy.Admin);

            endpoints.MapDelete("api/management/categories/{id:long}", Delete)
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .WithTags(groupName)
                .RequireAuthorization(AuthorizationPolicy.Admin);
            
            return endpoints;
        }

        private static async ValueTask<IResult> GetAll(
            [FromServices] IQueryHandler<GetAllCategoriesQuery, List<CategoryReadModel>> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(new GetAllCategoriesQuery(), cancellationToken);
            return Results.Ok(result);
        }

        private static async ValueTask<IResult> GetById(
            [FromRoute] long id, 
            [FromServices] IQueryHandler<GetCategoryByIdQuery, CategoryReadModel> handler,
            CancellationToken cancellationToken)
        {
            var result = await handler.HandleAsync(
                new GetCategoryByIdQuery
                {
                    Id = id
                }, cancellationToken);
            return Results.Ok(result);
        }

        private static async ValueTask<IResult> Create(
            [FromBody] CreateCategoryRequest request, 
            [FromServices] ICommandHandler<CreateCategoryCommand, long> handler,
            CancellationToken cancellationToken)
        {
            var id = await handler.HandleAsync(
                new CreateCategoryCommand
                {
                    Name = request.Name
                }, cancellationToken);
            return Results.Created($"api/products/categories/{id}", null);
        }

        private static async ValueTask<IResult> Update(
            [FromRoute] long id,
            [FromBody] UpdateCategoryRequest request,
            [FromServices] ICommandHandler<UpdateCategoryCommand> handler,
            CancellationToken cancellationToken)
        {
            await handler.HandleAsync(
                new UpdateCategoryCommand
                {
                    Id = id, Name = request.Name
                }, cancellationToken);
            return Results.NoContent();
        }

        private static async ValueTask<IResult> Delete(
            [FromRoute] long id,
            [FromServices] ICommandHandler<DeleteCategoryCommand> handler,
            CancellationToken cancellationToken)
        {
            await handler.HandleAsync(
                new DeleteCategoryCommand
                {
                    Id = id
                }, cancellationToken);
            return Results.NoContent();
        }
    }
}
