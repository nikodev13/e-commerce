using ECommerce.API.Endpoints.RequestBodies;
using ECommerce.ApplicationCore.Features.Customers.Commands;
using ECommerce.ApplicationCore.Features.Customers.Queries;
using ECommerce.ApplicationCore.Features.Customers.ReadModels;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder RegisterCustomersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Customer account";
        
        endpoints.MapGet("api/customers", GetAccount)
            .Produces<CustomerAccountReadModel>()
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(groupName);
        
        endpoints.MapPost("api/customers/register", Register)
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization()
            .WithTags(groupName);
        
        endpoints.MapPut("api/customers/account/update-fullname", UpdateFullName)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(groupName);

        endpoints.MapPost("api/customers/account/update-contact-data", UpdateContactData)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(groupName);

        return endpoints;
    }
    
    private static async ValueTask<IResult> GetAccount(
        [FromServices] IQueryHandler<GetCustomerAccountQuery, CustomerAccountReadModel> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(GetCustomerAccountQuery.Instance, cancellationToken);
        return Results.Ok(result);
    }
    
    private static async ValueTask<IResult> Register(
        [FromBody] RegisterCustomerRequestBody requestBody,
        [FromServices] ICommandHandler<RegisterCustomerCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(requestBody.ToCommand(), cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> UpdateFullName(
        [FromBody] UpdateCustomerFullNameRequestBody requestBody,
        [FromServices] ICommandHandler<UpdateCustomerFullNameCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(requestBody.ToCommand(), cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> UpdateContactData(
        [FromBody] UpdateCustomerContactDataRequestBody requestBody,
        [FromServices] ICommandHandler<UpdateCustomerContactDataCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(requestBody.ToCommand(), cancellationToken);
        return Results.NoContent();
    }
}