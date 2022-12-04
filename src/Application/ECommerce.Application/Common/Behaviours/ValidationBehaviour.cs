using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Results.Errors;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace ECommerce.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResult> : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
    where TResult : Result
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        var validationFailures = _validators
            .Select(x => x.Validate(request))
            .SelectMany(x => x.Errors)
            .ToList();

        if (validationFailures.Count != 0)
        {
            return CreateValidationError(validationFailures);
        }
        
        return await next();
    }
    
    private static TResult CreateValidationError(List<ValidationFailure> validationFailures)
    {
        if (typeof(TRequest) == typeof(Result))
            return (TResult)new ValidationError(validationFailures);

        var methodInfo = typeof(TResult).GetMethod("Failure")!;
        var validationError = methodInfo.Invoke(null, new object[] { new ValidationError(validationFailures) })!;

        return (TResult)validationError;
    }
}