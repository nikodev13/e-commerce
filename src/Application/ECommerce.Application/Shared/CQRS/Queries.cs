using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Shared.CQRS;

public interface IQuery<out TResult> { }

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    ValueTask<TResult> HandleAsync(TQuery command, CancellationToken cancellationToken);
}