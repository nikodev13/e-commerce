using MediatR;

namespace ECommerce.Application.Shared.CQRS;

public interface ICommand : IRequest
{
}

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : ICommand
{
}

public interface ICommand<TResult> : IRequest<TResult>
{
}

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
}