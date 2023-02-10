using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Shared.CQRS;

public interface IQuery<out TResult> { }

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    ValueTask<TResult> Handle(TQuery command, CancellationToken cancellationToken);
}

public interface IQueryDispatcher
{
    ValueTask<TResult> Dispatch<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
}