using ECommerce.Infrastructure.Authentication;
using ECommerce.Infrastructure.Authorization;
using ECommerce.Infrastructure.Logging;
using ECommerce.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class Dependencies
{
    public static void ConfigureInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigurePersistence(configuration);
        services.ConfigureAuthentication(configuration);
        services.ConfigureAuthorization();
        services.ConfigureSerilog();
    }
}