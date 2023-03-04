using ECommerce.API.Requests;
using ECommerce.ApplicationCore.Features.Customer.Adresses;
using ECommerce.ApplicationCore.Features.Customer.Adresses.Commands;
using ECommerce.ApplicationCore.Features.Customer.Adresses.Queries;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints.Customers;

public static class AddressEndpoints
{
    public static void RegisterCustomerAddressEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Customer Addresses"; 
                
        endpoints.MapGet("api/customers/addresses", GetAll)
            .Produces<List<AddressReadModel>>()
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(groupName);
        
        endpoints.MapPost("api/customers/addresses", Create)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(groupName);
        
        endpoints.MapPut("api/customers/addresses/{id:long}", Update)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(groupName);
        
        endpoints.MapDelete("api/customers/addresses/{id:long}", Delete)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(groupName);
    }
    
    private static async Task<IResult> GetAll(
        [FromServices] IQueryHandler<GetAllAddressesCommand, List<AddressReadModel>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetAllAddressesCommand(), cancellationToken);
        return Results.Ok(result);
    }
    
    private static async ValueTask<IResult> Create(
        [FromBody] AddNewAddressRequest request,
        [FromServices] ICommandHandler<AddNewAddressCommand, long> handler,
        CancellationToken cancellationToken)
    {
        var command = new AddNewAddressCommand
        {
            Street = request.Street,
            PostalCode = request.PostalCode,
            City = request.City
        };
        var result = await handler.HandleAsync(command, cancellationToken);
        return Results.Created($"api/customers/addresses/", null);
    }
    
    private static async ValueTask<IResult> Update(
        [FromRoute] long id,
        [FromBody] UpdateAddressRequest request,
        [FromServices] ICommandHandler<UpdateAddressCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAddressCommand
        {
            Id = id,
            Street = request.Street,
            PostalCode = request.PostalCode,
            City = request.City
        };
        await handler.HandleAsync(command, cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> Delete(
        [FromRoute] long id,
        [FromServices] ICommandHandler<DeleteAddressCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new DeleteAddressCommand { Id = id }, cancellationToken);
        return Results.NoContent();
    }
}