using System.Linq.Expressions;
using ECommerce.Application.Shared.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Processing;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    public CommandDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;
    
    public async ValueTask DispatchAsync<TCommand>(TCommand command, CancellationToken cancellationToken) 
        where TCommand : ICommand
    {
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        await handler.HandleAsync(command, cancellationToken);
    }

    public async ValueTask<TResult> DispatchAsync<TCommand, TResult>(TCommand command, CancellationToken cancellationToken)
        where TCommand : ICommand<TResult>
    {
        var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
        return await handler.HandleAsync(command, cancellationToken);
    }
    
}