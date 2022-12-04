using FluentValidation.Results;

namespace ECommerce.Application.Common.Results.Errors;

public class ValidationError : ErrorBase
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationError() : base("One or more validation failures have occured.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationError(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }

}