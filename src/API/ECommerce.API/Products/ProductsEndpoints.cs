using ECommerce.Application.Products.Commands;
using ECommerce.Application.Products.Queries;
using ECommerce.Application.Products.ReadModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Products;

public static class ProductsEndpoints
{
    private static async Task<IResult> GetAll([FromServices] IMediator mediator)
    {
        var query = new GetAllProductsQuery();
        var result = await mediator.Send(query);
        return Results.Ok(result);
    } 
    
    private static async Task<IResult> GetById([FromServices] IMediator mediator, [FromRoute] long id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await mediator.Send(query);
        return Results.Ok(result);
    }

    private static async Task<IResult> Create([FromServices] IMediator mediator,
        [FromBody] CreateProductCommand command)
    {
        var result = await mediator.Send(command);
        return Results.Created($"api/products/{result.Id}", result);
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