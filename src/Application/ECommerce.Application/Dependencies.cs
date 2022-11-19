using System.Reflection;
using ECommerce.Application.Shared.Services;
using ECommerce.Domain.Shared.Services;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace ECommerce.Application;

public static class Dependencies
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddSingleton<ISnowflakeIdService, SnowflakeIdService>();
                    
        return services;
    }
}