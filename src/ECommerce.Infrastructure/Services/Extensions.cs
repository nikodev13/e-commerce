using ECommerce.ApplicationCore.Shared.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Services;

internal static class Extensions
{
    internal static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ISnowflakeIdProvider, SnowflakeIdProvider>();
        
        return services;
    }
}