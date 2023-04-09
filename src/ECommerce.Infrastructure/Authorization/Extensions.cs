using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ECommerce.Infrastructure.Authorization;

internal static class Extensions
{
    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationHandler, AdminRoleRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, RegisteredCustomerRequirementHandler>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicy.Admin, policy =>
            {
                policy.AddRequirements(new AdminRoleRequirement());
            });
            
            options.AddPolicy(AuthorizationPolicy.RegisteredCustomer, policy =>
            {
                policy.AddRequirements(new RegisteredCustomerRequirement());
            });
        });

        services.AddSingleton<IAuthorizationMiddlewareResultHandler, CustomAuthorizationMiddlewareResultHandler>();
    }
}