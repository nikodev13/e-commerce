using MediatR;

namespace ECommerce.Application.Common.CQRS;

public interface IQuery<out TResult> : IRequest<TResult>
{
}

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IRequest<TResult>
{
}