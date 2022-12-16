using ECommerce.API.Utilities;
using ECommerce.Application.Products.Commands;
using ECommerce.Application.Products.Queries;
using ECommerce.Application.Products.ReadModels;
using ECommerce.Domain.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Products;

public static class ProductsEndpoints
{
    private static async Task<IResult> GetAll([FromServices] IMediator mediator)
    {
        var query = new GetAllProductsQuery();
        var result = await mediator.Send(query);
        return result.Resolve(Results.Ok);
    } 
    
    private static async Task<IResult> GetById([FromServices] IMediator mediator, [FromRoute] long id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await mediator.Send(query);
        return result.Resolve(Results.Ok);
    }

    private static async Task<IResult> Create([FromServices] IMediator mediator,
        [FromBody] CreateProductCommand command)
    {
        var result = await mediator.Send(command);
        return result.Resolve(() => Results.Created($"api/products/{result.Value.Id}", result.Value));
    }
    
    public static void RegisterProductEndpoints(this WebApplication app)
    {
        app.MapGet("api/products", GetAll)
            .Produces<List<ProductReadModel>>();
        
        app.MapGet("api/products/{id:long}", GetById)
            .Produces<ProductReadModel>()
            .Produces(StatusCodes.Status404NotFound);

        app.MapPost("api/products", Create)
            .Produces<ProductReadModel>()
            .Produces(StatusCodes.Status400BadRequest);
    }
}