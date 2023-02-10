using ECommerce.Infrastructure.Authentication;
using ECommerce.Infrastructure.Authorization;
using ECommerce.Infrastructure.Logging;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Processing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class Extensions
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureProcessing();
        services.ConfigurePersistence(configuration);
        services.ConfigureAuthentication(configuration);
        services.ConfigureAuthorization();
        services.ConfigureSerilog();

        return services;
    }
}