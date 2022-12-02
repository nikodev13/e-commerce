using ECommerce.API.Utilities;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Users;

public static class UsersEndpoints
{
    [AllowAnonymous]
    private static async Task<IResult> Register([FromServices] IMediator mediator, [FromBody] RegisterRequest request)
    {
        var command = new RegisterUserCommand(request.Email, request.Password, request.ConfirmPassword);
        var result = await mediator.Send(command);
        return result.Resolve(() => Results.Ok());
    }
    
    public static WebApplication RegisterUserEndpoints(this WebApplication app)
    {
        app.MapPost("api/users/register", Register);
        
        return app;
    }
}