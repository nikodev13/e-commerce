using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using ValidationException = ECommerce.ApplicationCore.Shared.Exceptions.ValidationException;

namespace ECommerce.ApplicationCore.Shared.Decorators;

public class ValidationCommandHandlerDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> 
    where TCommand : ICommand<TResult>
{
    private readonly ICommandHandler<TCommand, TResult> _handler;
    private readonly IValidator<TCommand> _validator;

    public ValidationCommandHandlerDecorator(ICommandHandler<TCommand, TResult> handler, 
        IValidator<TCommand> validator)
    {
        _handler = handler;
        _validator = validator;
    }

    public ValueTask<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(command);
        
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return _handler.HandleAsync(command, cancellationToken);
    }
}

public class ValidationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> 
    where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _handler;
    private readonly IValidator<TCommand> _validator;

    public ValidationCommandHandlerDecorator(ICommandHandler<TCommand> handler, IValidator<TCommand> validator)
    {
        _handler = handler;
        _validator = validator;
    }

    public ValueTask HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        var validationResult = _validator.Validate(command);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return _handler.HandleAsync(command, cancellationToken);
    }
}