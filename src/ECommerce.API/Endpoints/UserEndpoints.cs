using ECommerce.API.Endpoints.Requests;
using ECommerce.ApplicationCore.Features.Users;
using ECommerce.ApplicationCore.Features.Users.Commands;
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
        [FromBody] RegisterUserRequest request,
        [FromServices] ICommandHandler<RegisterUserCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(request.ToCommand(), cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> Login(
        [FromBody] LoginUserRequest request, 
        [FromServices] ICommandHandler<LoginUserCommand, TokensReadModel> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request.ToCommand(), cancellationToken);
        return Results.Ok(result);
    }
    
    private static async ValueTask<IResult> RefreshToken(
        [FromBody] RefreshTokenRequest request,
        [FromServices] ICommandHandler<RefreshTokenCommand, TokensReadModel> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request.ToCommand(), cancellationToken);
        return Results.Ok(result);
    }
}