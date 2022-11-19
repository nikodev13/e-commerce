using System.Diagnostics;
using ECommerce.Application.Shared.Results;
using ECommerce.Application.Shared.Results.Errors;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Utilities;

public static class EndpointsExtensions
{
    internal static IResult Resolve(this Result result, Func<IResult> httpResultFunc)
    {
        return result.Match(httpResultFunc, HandleError);
    }
    
    internal static IResult Resolve<T>(this Result<T> result, Func<T, IResult> httpResultFunc)
    {
        return result.Match(httpResultFunc, HandleError);
    }

    private static IResult HandleError(ErrorBase error)
    {
        return error switch
        {
            NotFoundError notFoundError => Results.NotFound(notFoundError.Message),
            AlreadyExistsError alreadyExistsError => Results.Conflict(alreadyExistsError.Message),
            _ => Results.StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}