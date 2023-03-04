using ECommerce.API.Requests;
using ECommerce.ApplicationCore.Features.Customer.Account;
using ECommerce.ApplicationCore.Features.Customer.Account.Commands;
using ECommerce.ApplicationCore.Features.Customer.Account.Queries;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints.Customers;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder RegisterCustomerAccountEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Customer account";
        
        endpoints.MapGet("api/customers/register", GetAccount)
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
        var result = await handler.HandleAsync(new GetCustomerAccountQuery(), cancellationToken);
        return Results.Ok(result);
    }
    
    private static async ValueTask<IResult> Register(
        [FromBody] RegisterCustomerAccountRequest request,
        [FromServices] ICommandHandler<RegisterCustomerCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCustomerCommand
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };
        await handler.HandleAsync(command, cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> UpdateFullName(
        [FromBody] UpdateCustomerFullNameRequest request,
        [FromServices] ICommandHandler<UpdateCustomerFullNameCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCustomerFullNameCommand()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
        };
        await handler.HandleAsync(command, cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> UpdateContactData(
        [FromBody] UpdateCustomerContactDataRequest request,
        [FromServices] ICommandHandler<UpdateCustomerContactDataCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCustomerContactDataCommand()
        {
            Email = request.Email,
            PhoneNumber = request.PhoneNumber
        };
        await handler.HandleAsync(command, cancellationToken);
        return Results.NoContent();
    }

}