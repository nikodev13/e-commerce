﻿using FluentValidation.Results;

namespace ECommerce.Application.Shared.Exceptions;

public class ValidationException : ApplicationException
{
    public IDictionary<string, string[]> Errors { get; }
    
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures have occurred.")
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
}