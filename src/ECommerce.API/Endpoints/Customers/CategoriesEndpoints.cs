using ECommerce.ApplicationCore.Features.Customers.Categories;
using ECommerce.ApplicationCore.Features.Customers.Categories.Queries;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints.Customers;

public static class CategoriesEndpoints
{
    public static IEndpointRouteBuilder RegisterCustomerCategoriesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Customer Categories"; 
        
        endpoints.MapGet("api/customers/categories", GetAllCustomerCategories)
            .WithTags(groupName);
        
        return endpoints;
    }

    private static async ValueTask<IResult> GetAllCustomerCategories(
        [FromServices] IQueryHandler<GetAllCustomerCategoriesQuery, List<CustomerCategoryReadModel>> handler,
        CancellationToken cancellationToken)

    {
        var readModels = await handler.HandleAsync(new GetAllCustomerCategoriesQuery(), cancellationToken);
        return Results.Ok(readModels);
    }
}