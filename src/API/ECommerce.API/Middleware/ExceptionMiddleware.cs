using ECommerce.Domain.SeedWork;
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
        catch (BusinessRuleValidationException ex)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError("{@1} \n {@2}", ex.Message, ex.StackTrace);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Internal server error.");
        }
    }
    
}