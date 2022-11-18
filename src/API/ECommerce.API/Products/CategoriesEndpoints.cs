using ECommerce.API.Utilities;
using ECommerce.Application.Categories;
using ECommerce.Application.Categories.Commands;
using ECommerce.Application.Categories.Queries;
using ECommerce.Domain.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Products
{
    public static class CategoriesEndpoints
    {
        private static async Task<IResult> GetAll([FromServices] IMediator mediator)
        {
            var result = await mediator.Send(new GetAllCategoriesQuery());
            return result.Resolve(x => Results.Ok(x));
        }

        private static async Task<IResult> GetById([FromServices] IMediator mediator, [FromRoute] Guid id)
        {
            var result = await mediator.Send(new GetCategoryByIdQuery(id));
            return result.Resolve(x => Results.Ok(x));
        }

        private static async Task<IResult> GetByName([FromServices] IMediator mediator, [FromRoute] string name)
        {
            var result = await mediator.Send(new GetCategoryByNameQuery(name));
            return result.Resolve(x => Results.Ok(x));
        }

        private static async Task<IResult> Create([FromServices] IMediator mediator, [FromBody] CategoryNameRequest nameRequest)
        {
            var result = await mediator.Send(new CreateCategoryCommand(nameRequest.CategoryName));
            return result.Resolve(x => Results.Created($"api/categories/{x}", null));
        }

        private static async Task<IResult> Update([FromServices] IMediator mediator, [FromRoute] Guid id, [FromBody] CategoryNameRequest nameRequest)
        {
            var result = await mediator.Send(new UpdateCategoryCommand(id, nameRequest.CategoryName));
            return result.Resolve(x => Results.NoContent());
        }

        private static async Task<IResult> Delete([FromServices] IMediator mediator, [FromRoute] Guid id)
        {
            var result = await mediator.Send(new DeleteCategoryCommand(id));
            return result.Resolve(x => Results.NoContent());
        }

        public static void RegisterCategoryEndpoints(this WebApplication app)
        {
            app.MapGet("api/categories", GetAll)
                .Produces<List<CategoryDto>>();

            app.MapGet("api/categories/{id}", GetById)
                .Produces<CategoryDto>()
                .Produces(StatusCodes.Status404NotFound);

            app.MapGet("api/categories/{name}", GetByName)
                .Produces<CategoryDto>()
                .Produces(StatusCodes.Status404NotFound);

            app.MapPost("api/categories", Create)
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status409Conflict);

            app.MapPut("api/categories/{id}", Update)
                 .Produces(StatusCodes.Status204NoContent)
                 .Produces(StatusCodes.Status404NotFound);

            app.MapDelete("api/categories/{id}", Delete)
                 .Produces(StatusCodes.Status204NoContent)
                 .Produces(StatusCodes.Status404NotFound);
        }
    }
}
