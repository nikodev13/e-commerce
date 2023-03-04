using ECommerce.ApplicationCore.Shared.CQRS;

namespace ECommerce.API.Configuration;

public static class EndpointsExtensions
{
    public static RouteHandlerBuilder MapPostCommand<TCommand>(this IEndpointRouteBuilder endpoint, string pattern,
        Func<TCommand, ICommandHandler<TCommand>, CancellationToken, ValueTask<IResult>> func) where TCommand : ICommand
    {
        return endpoint.MapPost(pattern, func);
    }
}