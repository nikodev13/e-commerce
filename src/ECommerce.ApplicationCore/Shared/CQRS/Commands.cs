namespace ECommerce.ApplicationCore.Shared.CQRS;

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