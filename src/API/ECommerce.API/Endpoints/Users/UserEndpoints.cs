using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Users;
using ECommerce.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Endpoints.Users;

public static class UsersEndpoints
{
    [AllowAnonymous]
    private static IResult CheckLogin([FromServices] IMediator mediator, [FromServices] IUserContextProvider contextProvider)
    {
        return Results.Ok(contextProvider.UserId);
    }
    
    public static IEndpointRouteBuilder RegisterUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        const string groupName = "Users";
        
        endpoints.MapPost("api/users/register",
        async (IMediator mediator, [FromBody] RegisterRequest request) =>
            {
                await mediator.Send(new RegisterUserCommand()
                {
                    Email = request.Email,
                    Password = request.Password
                });
                return Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .WithTags(groupName);
        
        endpoints.MapPost("api/users/login",            
        async (IMediator mediator, [FromBody] LoginRequest request) =>
            {
                var result = await mediator.Send(new LoginUserCommand()
                {
                    Email = request.Email,
                    Password = request.Password
                });
                return Results.Ok(result);
            })
            .Produces<TokensReadModel>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(groupName);

        endpoints.MapPost("api/users/refresh-token",            
        async (IMediator mediator, [FromBody] RefreshTokenRequest request) =>
            {
                var result = await mediator.Send(new RefreshTokenCommand()
                {
                    Email = request.Email,
                    RefreshToken = request.RefreshToken
                });
                return Results.Ok(result);
            })
            .Produces<TokensReadModel>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(groupName);
            
        
        endpoints.MapPost("api/users/checklogin", CheckLogin);
        
        return endpoints;
    }
}