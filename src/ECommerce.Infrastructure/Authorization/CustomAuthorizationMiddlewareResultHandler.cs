using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Infrastructure.Authorization;

public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private static readonly AuthorizationMiddlewareResultHandler DefaultHandler = new();
    
    public async Task HandleAsync(RequestDelegate next, HttpContext context, Microsoft.AspNetCore.Authorization.AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Forbidden)
        {
            var failure = authorizeResult.AuthorizationFailure!.FailureReasons.FirstOrDefault(failure => failure.Handler is RegisteredCustomerRequirementHandler);
            if (failure is not null)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync(failure.Message);
                return;
            }
        }

        // Fall back to the default implementation.
        await DefaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}