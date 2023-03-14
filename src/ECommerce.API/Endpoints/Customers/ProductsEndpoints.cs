using ECommerce.API.Endpoints.Customers.Requests;
using ECommerce.ApplicationCore.Features.Customers.Products.Queries;
using ECommerce.ApplicationCore.Features.Customers.Products.ReadModels;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints.Customers;

public static class ProductsEndpoints
{
    public static IEndpointRouteBuilder RegisterCustomerProductsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Customer Products";
        
        endpoints.MapGet("api/customers/products", GetPaginated)
            .WithTags(groupName);
        endpoints.MapGet("api/customers/products/{id}", GetById)
            .WithTags(groupName);
        endpoints.MapGet("api/customers/products/{id}/quantity",GetProductQuantityById)
            .WithTags(groupName);
        
        return endpoints;
    }

    private static async ValueTask<IResult> GetPaginated(
        [AsParameters] GetPaginatedCustomerProductsRequest request,
        [FromServices] IQueryHandler<GetPaginatedCustomerProductsQuery, PaginatedList<CustomerProductReadModel>> handler,
        CancellationToken cancellationToken)
    {
        var paginatedList = await handler.HandleAsync(request, cancellationToken);
        return Results.Ok(paginatedList);
    }

    private static async ValueTask<IResult> GetById(
        [FromRoute] long id,
        [FromServices] IQueryHandler<GetCustomerProductByIdQuery, CustomerProductReadModel> handler,
        CancellationToken cancellationToken)
    {
        var readModel = await handler.HandleAsync(new GetCustomerProductByIdQuery
        {
            Id = id
        }, cancellationToken) ;
        return Results.Ok(readModel);
    }

    private static async ValueTask<IResult> GetProductQuantityById(
        [FromRoute] long id,
        [FromServices] IQueryHandler<GetProductQuantityByIdQuery, CustomerProductQuantityReadModel> handler,
        CancellationToken cancellationToken)
    {
        var readModel = await handler.HandleAsync(new GetProductQuantityByIdQuery
        {
            Id = id
        }, cancellationToken);
        return Results.Ok(readModel);
    }
}