using ECommerce.Application.Common.Results;
using MediatR;

namespace ECommerce.Application.Common.CQRS;

public interface ICommand : IRequest<Result>
{
}

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

public interface ICommand<TResult> : IRequest<Result<TResult>>
{
}

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, Result<TResult>>
    where TCommand : ICommand<TResult>
{
}