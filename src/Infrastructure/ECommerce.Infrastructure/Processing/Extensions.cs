using ECommerce.Application.Shared.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Processing;

internal static class Extensions
{
    internal static IServiceCollection ConfigureProcessing(this IServiceCollection services)
    {
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        
        return services;
    }
}