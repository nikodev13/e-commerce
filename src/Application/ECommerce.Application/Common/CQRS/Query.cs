using ECommerce.Application.Common.Results;
using MediatR;

namespace ECommerce.Application.Common.CQRS;

public interface IQuery<TResult> : IRequest<Result<TResult>>
{
}

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, Result<TResult>>
    where TQuery : IRequest<Result<TResult>>
{
}