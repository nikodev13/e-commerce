using System.Diagnostics;
using ECommerce.Application.Shared.Results;
using ECommerce.Application.Shared.Results.Errors;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Utilities;

public static class ControllerExtensions
{
    public static IActionResult Resolve<T>(this Result<T> result, int successStatusCode = StatusCodes.Status200OK)
    {
        return result.Match<IActionResult>(resultValue =>
        {
            var successResult = new ObjectResult(resultValue)
            {
                StatusCode = successStatusCode
            };
            return successResult;
        }, error =>
        {
            return error switch
            {
                NotFoundError notFoundError => new ObjectResult(notFoundError.Message)
                {
                    StatusCode = StatusCodes.Status404NotFound,
                },
                AlreadyExistsError alreadyExistsError => new ObjectResult(alreadyExistsError.Message)
                {
                    StatusCode = StatusCodes.Status409Conflict,
                },
                _ => new ObjectResult("Internal server error")
                {
                    StatusCode = 500
                }
            };
        });
    }
}