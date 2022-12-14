using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ECommerce.Infrastructure.Authorization;

internal static class IdentityConfigurations
{
    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddScoped<IAuthorizationHandler, AdminRoleRequirementHandler>();
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicy.Admin,
                policy =>
                {
                    policy.AddRequirements(new AdminRoleRequirement());
                });
        });
    }
}