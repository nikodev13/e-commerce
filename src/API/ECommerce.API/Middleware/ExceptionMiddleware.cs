using ECommerce.Application.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new ValidationProblemDetails(exception.Errors));
        }
        catch (BadRequestException exception)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(exception.Message);
        }
        catch (NotFoundException exception)
        {
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(exception.Message);
        }
        catch (AlreadyExistsException exception)
        {
            context.Response.StatusCode = 409;
            await context.Response.WriteAsync(exception.Message);
        }
        catch (Exception exception)
        {
            _logger.LogCritical(exception, exception.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Internal server error.");
        }
    }
    
}