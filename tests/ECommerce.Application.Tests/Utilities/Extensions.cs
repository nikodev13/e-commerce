using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Tests.Utilities;

public static class Extensions
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