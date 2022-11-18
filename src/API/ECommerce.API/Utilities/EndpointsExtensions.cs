using System.Diagnostics;
using ECommerce.Application.Shared.Results;
using ECommerce.Application.Shared.Results.Errors;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Utilities;

public static class EndpointsExtensions
{
    public static IResult Resolve<T>(this Result<T> result, Func<T, IResult> httpResultFunc)
    {
        return result.Match<IResult>(resultValue =>
        {
            return httpResultFunc(resultValue);
        }, error =>
        {
            return Results.BadRequest();
            //return error switch
            //{
            //    NotFoundError notFoundError => new ObjectResult(notFoundError.Message)
            //    {
            //        StatusCode = StatusCodes.Status404NotFound,
            //    },
            //    AlreadyExistsError alreadyExistsError => new ObjectResult(alreadyExistsError.Message)
            //    {
            //        StatusCode = StatusCodes.Status409Conflict,
            //    },
            //    _ => new ObjectResult("Internal server error")
            //    {
            //        StatusCode = 500
            //    }
            //};
        });
    }
}