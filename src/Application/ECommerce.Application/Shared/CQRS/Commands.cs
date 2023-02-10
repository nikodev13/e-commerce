namespace ECommerce.Application.Shared.CQRS;

public interface ICommand { }
public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    ValueTask HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface ICommand<out TResult> { }
public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
{
    ValueTask<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandDispatcher
{
    ValueTask Dispatch<TCommand>(TCommand query, CancellationToken cancellationToken)
        where TCommand : ICommand;

    ValueTask<TResult> Dispatch<TResult>(ICommand<TResult> query, CancellationToken cancellationToken);
}