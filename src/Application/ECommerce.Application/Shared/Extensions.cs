using ECommerce.Application.Shared.CQRS;
using ECommerce.Application.Shared.Decorators;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application.Shared;

public static class Extensions
{
    public static IServiceCollection ConfigureSharedServices(this IServiceCollection services)
    {
        var assembly = typeof(Extensions).Assembly;

        services.Scan(selector => selector.FromAssemblies(assembly)
            .AddClasses(filter => filter
                .AssignableToAny(typeof(IQueryHandler<,>), typeof(ICommandHandler<>), typeof(ICommandHandler<,>))
                .Where(x => x.Namespace != typeof(ValidationQueryHandlerDecorator<,>).Namespace))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // services.Decorate(typeof(IQueryHandler<,>), typeof(ValidationQueryHandlerDecorator<,>));
        // services.Decorate(typeof(ICommandHandler<>), typeof(ValidationCommandHandlerDecorator<>));
        // services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));
        
        return services;
    }
}