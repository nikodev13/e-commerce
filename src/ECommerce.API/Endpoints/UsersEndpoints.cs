using ECommerce.API.Endpoints.RequestBodies;
using ECommerce.ApplicationCore.Features.Users;
using ECommerce.ApplicationCore.Features.Users.Commands;
using ECommerce.ApplicationCore.Features.Users.Queries;
using ECommerce.ApplicationCore.Features.Users.ReadModels;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Models;
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

        endpoints.MapGet("api/users/{email}", GetUserRole)
            .Produces<UserRoleReadModel>()
            .WithTags(groupName);
        
        endpoints.MapPost("api/users/login", Login)
            .Produces<TokensReadModel>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(groupName);

        endpoints.MapPost("api/users/refresh-token", RefreshToken)
            .Produces<TokensReadModel>()
            .Produces(StatusCodes.Status400BadRequest)
            .WithTags(groupName);
        
        endpoints.MapPatch("api/users/change-email", ChangeUserEmail)
            .Produces<PaginatedList<UserInListReadModel>>()
            .WithTags(groupName)
            .RequireAuthorization();
        
        endpoints.MapGet("api/users", GetPaginatedUsers)
            .Produces<PaginatedList<UserInListReadModel>>()
            .WithTags(groupName)
            .RequireAuthorization();
            
        return endpoints;
    }

    private static async ValueTask<IResult> GetUserRole(
        [FromRoute] string email,
        [FromServices] IQueryHandler<GetUserRoleQuery, UserRoleReadModel> handler,
        CancellationToken cancellationToken)
    {
        var role = await handler.HandleAsync(new GetUserRoleQuery(email), cancellationToken);
        return Results.Ok(role);
    }
    
    private static async ValueTask<IResult> GetPaginatedUsers(
        [AsParameters] GetPaginatedUsersRequestParameters parameters,
        [FromServices] IQueryHandler<GetPaginatedUsersQuery, PaginatedList<UserInListReadModel>> handler,
        CancellationToken cancellationToken)
    {
        var users = await handler.HandleAsync(parameters.ToQuery(), cancellationToken);
        return Results.Ok(users);
    }
    
    private static async ValueTask<IResult> Register(
        [FromBody] RegisterUserRequestBody body,
        [FromServices] ICommandHandler<RegisterUserCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(body.ToCommand(), cancellationToken);
        return Results.NoContent();
    }
    
    private static async ValueTask<IResult> Login(
        [FromBody] LoginUserRequestBody body, 
        [FromServices] ICommandHandler<LoginUserCommand, TokensReadModel> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(body.ToCommand(), cancellationToken);
        return Results.Ok(result);
    }
    
    private static async ValueTask<IResult> RefreshToken(
        [FromBody] RefreshTokenRequestBody body,
        [FromServices] ICommandHandler<RefreshTokenCommand, TokensReadModel> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(body.ToCommand(), cancellationToken);
        return Results.Ok(result);
    }
    
    private static async ValueTask<IResult> ChangeUserEmail(
        [FromBody] ChangeUserEmailRequestBody body,
        [FromServices] ICommandHandler<ChangeUserEmailCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(body.ToCommand(), cancellationToken);
        return Results.NoContent();
    }
}