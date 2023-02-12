using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Shared.CQRS;
using ECommerce.Application.Users;
using ECommerce.Application.Users.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints.Users;

public static class UsersEndpoints
{
    [AllowAnonymous]
    private static IResult CheckLogin([FromServices] IUserContextProvider contextProvider)
    {
        return Results.Ok(contextProvider.UserId);
    }
    
    public static IEndpointRouteBuilder RegisterUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Users";
        
        endpoints.MapPost("api/users/register",
        async ([FromBody] RegisterRequest request, [FromServices] ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
            {
                await dispatcher.DispatchAsync(new RegisterUserCommand
                {
                    Email = request.Email,
                    Password = request.Password
                }, cancellationToken);
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .WithTags(groupName);
        
        endpoints.MapPost("api/users/login",            
        async ([FromBody] LoginRequest request, [FromServices] ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
            {
                var result = await dispatcher.DispatchAsync<LoginUserCommand, TokensReadModel>(new LoginUserCommand
                {
                    Email = request.Email,
                    Password = request.Password
                }, cancellationToken);
                return Results.Ok(result);
            })
            .Produces<TokensReadModel>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(groupName);

        endpoints.MapPost("api/users/refresh-token",            
        async ([FromBody] RefreshTokenRequest request, [FromServices] ICommandDispatcher dispatcher, CancellationToken cancellationToken) =>
            {
                var result = await dispatcher.DispatchAsync<RefreshTokenCommand, TokensReadModel>(new RefreshTokenCommand
                {
                    Email = request.Email,
                    RefreshToken = request.RefreshToken
                }, cancellationToken);
                return Results.Ok(result);
            })
            .Produces<TokensReadModel>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(groupName);
            
        
        endpoints.MapPost("api/users/checklogin", CheckLogin);
        
        return endpoints;
    }
}