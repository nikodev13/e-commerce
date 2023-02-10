using ECommerce.Application.Shared.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Processing;

public class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;
    
    public async ValueTask<TResult> Dispatch<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
    {
        var handler = _serviceProvider.GetRequiredService<IQueryHandler<IQuery<TResult>, TResult>>();
        return await handler.Handle(query, cancellationToken);
    }
}