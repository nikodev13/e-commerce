using ECommerce.Domain.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Domain;

public static class Extensions
{
    public static IServiceCollection RegisterDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ISnowflakeIdProvider, SnowflakeIdProvider>();

        return services;
    }
}