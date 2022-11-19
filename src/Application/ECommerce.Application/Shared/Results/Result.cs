using ECommerce.Application.Shared.Results.Errors;
using FluentValidation.Results;

namespace ECommerce.Application.Shared.Results;

public class Result
{
    private static readonly Result SuccessResult = new Result();
    public bool IsSuccess { get; }
    public ErrorBase? Error { get; }

    protected Result()
    {
        IsSuccess = true;
    }

    protected Result(ErrorBase error)
    {
        IsSuccess = false;
        Error = error;
    }

    public R Match<R>(Func<R> successPredicate, Func<ErrorBase, R> failurePredicate)
    {
        return IsSuccess ? successPredicate.Invoke() : failurePredicate(Error!);
    }
    
    public static Result Success() => SuccessResult;
    public static Result Failure(ErrorBase error) => new Result(error);
    public static implicit operator Result(ErrorBase error) => Failure(error);
}

public class Result<T> : Result
{
    public T? Value { get; }
    
    protected Result(T value)
    {
        Value = value;
    }

    protected Result(ErrorBase error) : base(error)
    {
    }

    public R Match<R>(Func<T, R> successPredicate, Func<ErrorBase, R> failurePredicate)
    {
        return IsSuccess ? successPredicate.Invoke(Value!) : failurePredicate(Error!);
    }

    public static Result<T> Success(T value) => new(value);
    public new static Result<T> Failure(ErrorBase error) => new(error);

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(ErrorBase error) => Failure(error);
}