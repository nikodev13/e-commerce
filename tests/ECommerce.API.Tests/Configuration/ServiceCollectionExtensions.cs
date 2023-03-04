using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.API.Tests.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection Remove<TService>(this IServiceCollection services)
    {
        var service = services.FirstOrDefault(x => x.ServiceType == typeof(TService));

        if (service is not null)
        {
            services.Remove(service);
        }

        return services;
    }
}