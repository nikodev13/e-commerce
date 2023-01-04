using ECommerce.API.Requests;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Users.Commands;
using ECommerce.Application.Users.ReadModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ECommerce.API.Endpoints;

public static class UsersEndpoints
{
    private static async Task<IResult> Register([FromServices] IMediator mediator, [FromBody] RegisterRequest request)
    {
        var command = new RegisterUserCommand(request.Email, request.Password, request.ConfirmPassword);
        await mediator.Send(command);
        return Results.NoContent();
    }
    
    private static async Task<IResult> Login([FromServices] IMediator mediator, [FromBody] LoginRequest request)
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var result = await mediator.Send(command);
        return Results.Ok(result);
    }
    
    [AllowAnonymous]
    private static IResult CheckLogin([FromServices] IMediator mediator, [FromServices] IUserContextService contextService)
    {
        return Results.Ok(contextService.User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
    }
    
    private static async Task<IResult> RefreshToken([FromServices] IMediator mediator, [FromBody] RefreshTokenCommand refreshTokenCommand)
    {
        var result = await mediator.Send(refreshTokenCommand);
        return Results.Ok(result);
    }

    public static WebApplication RegisterUserEndpoints(this WebApplication app)
    {
        app.MapPost("api/users/register", Register)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict);
        
        app.MapPost("api/users/login", Login)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);

        app.MapPost("api/users/refresh-token", RefreshToken)
            .Produces<TokensReadModel>()
            .Produces(StatusCodes.Status400BadRequest);
            
        
        app.MapPost("api/users/checklogin", CheckLogin);
        
        return app;
    }
}