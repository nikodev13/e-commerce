using ECommerce.API.Endpoints.Requests;
using ECommerce.ApplicationCore.Features.CustomerAccounts;
using ECommerce.ApplicationCore.Features.CustomerAccounts.Commands;
using ECommerce.ApplicationCore.Features.CustomerAccounts.Queries;
using ECommerce.ApplicationCore.Features.CustomerAccounts.ReadModels;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

public static class AccountEndpoints
{
    public static IEndpointRouteBuilder RegisterCustomerAccountEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string accountGroupName = "Customer account";
        
        endpoints.MapGet("api/customers", GetAccount)
            .Produces<CustomerAccountReadModel>()
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(accountGroupName);
        
        endpoints.MapPost("api/customers/register", Register)
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization()
            .WithTags(accountGroupName);
        
        endpoints.MapPut("api/customers/account/update-fullname", UpdateFullName)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(accountGroupName);

        endpoints.MapPost("api/customers/account/update-contact-data", UpdateContactData)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(accountGroupName);

        const string addressesGroupName = "Customer addresses";
        
        endpoints.MapPost("api/customers/addresses", CreateAddress)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(accountGroupName);
        
        endpoints.MapPut("api/customers/addresses/{id:long}", UpdateAddress)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(accountGroupName);
        
        endpoints.MapDelete("api/customers/addresses/{id:long}", DeleteAddress)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(accountGroupName);

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
        [FromBody] RegisterCustomerAccountRequest request,
        [FromServices] ICommandHandler<RegisterCustomerAccountCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(request.ToCommand(), cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> UpdateFullName(
        [FromBody] UpdateCustomerFullNameRequest request,
        [FromServices] ICommandHandler<UpdateCustomerFullNameCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(request.ToCommand(), cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> UpdateContactData(
        [FromBody] UpdateCustomerContactDataRequest request,
        [FromServices] ICommandHandler<UpdateCustomerContactDataCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(request.ToCommand(), cancellationToken);
        return Results.NoContent();
    }

    private static async ValueTask<IResult> CreateAddress(
        [FromBody] CreateCustomerAddressRequest request,
        [FromServices] ICommandHandler<CreateCustomerAddressCommand, long> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request.ToCommand(), cancellationToken);
        return Results.Created($"api/customers/addresses/", result);
    }
    
    private static async ValueTask<IResult> UpdateAddress(
        [FromRoute] long id,
        [FromBody] UpdateCustomerAddressRequest request,
        [FromServices] ICommandHandler<UpdateCustomerAddressCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(request.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> DeleteAddress(
        [FromRoute] long id,
        [FromServices] ICommandHandler<DeleteCustomerAddressCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new DeleteCustomerAddressCommand(id), cancellationToken);
        return Results.NoContent();
    }
}