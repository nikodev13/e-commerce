using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Decorators;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.ApplicationCore.Shared;

public static class Extensions
{
    public static IServiceCollection ConfigureCqrs(this IServiceCollection services)
    {
        var assembly = typeof(Extensions).Assembly;

        services.Scan(selector => selector.FromAssemblies(assembly)
            .AddClasses(filter => filter
                .AssignableToAny(typeof(IQueryHandler<,>), typeof(ICommandHandler<>), typeof(ICommandHandler<,>))
                .Where(x => x.Namespace!.Contains("Features")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Decorate(typeof(IQueryHandler<,>), typeof(ValidationQueryHandlerDecorator<,>));
        services.Decorate(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));
        
        return services;
    }
}