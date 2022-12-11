using ECommerce.Application.Users.Models;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Authorization;

internal static class IdentityConfigurations
{
    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicy.Admin, policy => policy.RequireRole(nameof(Role.Admin)));
        });
    }
}