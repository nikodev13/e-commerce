using ECommerce.API.Endpoints.RequestBodies;
using ECommerce.ApplicationCore.Features.AddressBook.Commands;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

internal static class AddressBookEndpoints
{
    public static IEndpointRouteBuilder RegisterAddressBookEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Customer address book";
        
        endpoints.MapPost("api/address-book", AddAddress)
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(groupName);
        
        endpoints.MapPut("api/address-book/{id:long}", UpdateAddress)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(groupName);
        
        endpoints.MapDelete("api/address-book/{id:long}", DeleteAddress)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(AuthorizationPolicy.RegisteredCustomer)
            .WithTags(groupName);
        
        return endpoints;
    }
    
    private static async ValueTask<IResult> AddAddress(
        [FromBody] AddAddressToAddressBookRequestBody toAddressBookRequestBody,
        [FromServices] ICommandHandler<AddAddressToAddressBookCommand, long> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(toAddressBookRequestBody.ToCommand(), cancellationToken);
        return Results.Created($"api/address-book/", result);
    }
    
    private static async ValueTask<IResult> UpdateAddress(
        [FromRoute] long id,
        [FromBody] UpdateAddressInAddressBookRequestBody inAddressBookRequestBody,
        [FromServices] ICommandHandler<UpdateAddressInAddressBookCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(inAddressBookRequestBody.ToCommand(id), cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> DeleteAddress(
        [FromRoute] long id,
        [FromServices] ICommandHandler<DeleteAddressFromAddressBookCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(new DeleteAddressFromAddressBookCommand(id), cancellationToken);
        return Results.NoContent();
    }
}