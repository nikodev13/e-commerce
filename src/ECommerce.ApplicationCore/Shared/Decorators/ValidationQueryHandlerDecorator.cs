using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using ValidationException = ECommerce.ApplicationCore.Shared.Exceptions.ValidationException;

namespace ECommerce.ApplicationCore.Shared.Decorators;

public class ValidationQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult> 
    where TQuery : IQuery<TResult>
{
    private readonly IQueryHandler<TQuery, TResult> _handler;
    private readonly IValidator<TQuery> _validator;

    public ValidationQueryHandlerDecorator(IQueryHandler<TQuery, TResult> handler, IValidator<TQuery> validator)
    {
        _handler = handler;
        _validator = validator;
    }

    public ValueTask<TResult> HandleAsync(TQuery command, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(command);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return _handler.HandleAsync(command, cancellationToken);
    }
}