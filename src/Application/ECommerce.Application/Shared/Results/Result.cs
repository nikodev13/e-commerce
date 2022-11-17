using ECommerce.Application.Shared.Results.Errors;

namespace ECommerce.Application.Shared.Results;

public class Result<T>
{
    public T? Value { get; }
    public ErrorBase? Error { get; }
    public bool IsSuccess { get; }
    
    private Result(T value)
    {
        Value = value;
        IsSuccess = true;
    }

    private Result(ErrorBase error)
    {
        Error = error;
        IsSuccess = false;
    }

    public R Match<R>(Func<T, R> successPredicate, Func<ErrorBase, R> failurePredicate)
    {
        return IsSuccess ? successPredicate.Invoke(Value!) : failurePredicate(Error!);
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value);
    }
    
    public static Result<T> Failure(ErrorBase error)
    {
        return new Result<T>(error);
    }

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(ErrorBase error) => Failure(error);
}