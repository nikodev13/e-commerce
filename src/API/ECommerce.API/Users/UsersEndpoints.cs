using System.Security.Claims;
using ECommerce.API.Utilities;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Users.Commands;
using ECommerce.Application.Users.ReadModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ECommerce.API.Users;

public static class UsersEndpoints
{
    private static async Task<IResult> Register([FromServices] IMediator mediator, [FromBody] RegisterRequest request)
    {
        var command = new RegisterUserCommand(request.Email, request.Password, request.ConfirmPassword);
        var result = await mediator.Send(command);
        return result.Resolve(() => Results.Ok());
    }
    
    private static async Task<IResult> Login([FromServices] IMediator mediator, [FromBody] LoginRequest request)
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var result = await mediator.Send(command);
        return result.Resolve(Results.Ok);
    }
    
    [AllowAnonymous]
    private static IResult CheckLogin([FromServices] IMediator mediator, [FromServices] IUserContextService contextService)
    {
        return Results.Ok(contextService.User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
    }
    
    private static async Task<IResult> RefreshToken([FromServices] IMediator mediator, [FromBody] RefreshTokenCommand refreshTokenCommand)
    {
        var result = await mediator.Send(refreshTokenCommand);
        return result.Resolve(Results.Ok);
    }

    public static WebApplication RegisterUserEndpoints(this WebApplication app)
    {
        app.MapPost("api/users/register", Register)
            .Produces(StatusCodes.Status200OK)
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