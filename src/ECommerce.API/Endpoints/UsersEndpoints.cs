using ECommerce.API.Endpoints.RequestBodies;
using ECommerce.ApplicationCore.Features.Users;
using ECommerce.ApplicationCore.Features.Users.Commands;
using ECommerce.ApplicationCore.Features.Users.ReadModels;
using ECommerce.ApplicationCore.Shared.CQRS;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints;

public static class UsersEndpoints
{
    public static IEndpointRouteBuilder RegisterUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Users";
        
        endpoints.MapPost("api/users/register", Register)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .WithTags(groupName);
        
        endpoints.MapPost("api/users/login", Login)
            .Produces<TokensReadModel>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(groupName);

        endpoints.MapPost("api/users/refresh-token", RefreshToken)
            .Produces<TokensReadModel>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(groupName);
            
        return endpoints;
    }

    private static async ValueTask<IResult> Register(
        [FromBody] RegisterUserRequestBody requestBody,
        [FromServices] ICommandHandler<RegisterUserCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(requestBody.ToCommand(), cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> Login(
        [FromBody] LoginUserRequestBody requestBody, 
        [FromServices] ICommandHandler<LoginUserCommand, TokensReadModel> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(requestBody.ToCommand(), cancellationToken);
        return Results.Ok(result);
    }
    
    private static async ValueTask<IResult> RefreshToken(
        [FromBody] RefreshTokenRequestBody requestBody,
        [FromServices] ICommandHandler<RefreshTokenCommand, TokensReadModel> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(requestBody.ToCommand(), cancellationToken);
        return Results.Ok(result);
    }
}