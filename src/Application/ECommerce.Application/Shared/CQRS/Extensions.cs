using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Shared.CQRS;

public static class Extensions
{
    public static IServiceCollection RegisterApplicationHandlers(this IServiceCollection services)
    {
        var assembly = Assembly.GetAssembly(typeof(ECommerce.Application.Extensions))!;

        services.Scan(selector => selector.FromAssemblies(assembly)
            .AddClasses(filter => filter.AssignableToAny(typeof(IQueryHandler<,>),
                typeof(ICommandHandler<>), typeof(ICommandHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        
        return services;
    }
}