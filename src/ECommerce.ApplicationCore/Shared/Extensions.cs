using System.Reflection;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Decorators;
using FluentValidation;
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

        // services.Decorate(typeof(ICommandHandler<,>), typeof(ValidationCommandHandlerDecorator<,>));
        
        services.ConfigureQueryHandlersDecoratorsForValidation();
        services.ConfigureCommandHandlersDecoratorsForValidation();
        services.ConfigureCommandWithResultHandlersDecoratorsForValidation();
        
        return services;
    }

    private static void ConfigureQueryHandlersDecoratorsForValidation(this IServiceCollection services)
    {
        var assembly = typeof(Extensions).Assembly;
        var types = assembly.GetTypes();
        
        var queryHandlersTypes = types.Select(
                x => x.GetInterfaces().FirstOrDefault(@interface => @interface.IsGenericType
                                                                    && @interface.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
            .Where(x => x is not null).Cast<Type>().ToList();
        
        var validatableTypes = types
            .Where(x => x.BaseType?.IsGenericType == true && x.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
            .Select(x => x.BaseType!.GenericTypeArguments[0])
            .ToList();

        foreach (var queryHandler in queryHandlersTypes)
        {
            var queryType = queryHandler.GenericTypeArguments[0];
            var resultQueryType = queryHandler.GenericTypeArguments[1];
            if (validatableTypes.Contains(queryType))
            {
                var decorator = typeof(ValidationQueryHandlerDecorator<,>);
                services.Decorate(queryHandler, decorator.MakeGenericType(queryType, resultQueryType));
            }
        }
    }
    
    private static void ConfigureCommandHandlersDecoratorsForValidation(this IServiceCollection services)
    {
        var assembly = typeof(Extensions).Assembly;
        var types = assembly.GetTypes();
        
        var commandHandlersTypes = types.Select(
                x => x.GetInterfaces().FirstOrDefault(@interface => @interface.IsGenericType
                                                                    && @interface.GetGenericTypeDefinition() == typeof(ICommandHandler<>)))
            .Where(x => x is not null).Cast<Type>().ToList();
        
        var validatableTypes = types
            .Where(x => x.BaseType?.IsGenericType == true && x.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
            .Select(x => x.BaseType!.GenericTypeArguments[0])
            .ToList();
        
        foreach (var commandHandler in commandHandlersTypes)
        {
            var commandType = commandHandler.GenericTypeArguments[0];
            if (validatableTypes.Contains(commandType))
            {
                var decorator = typeof(ValidationCommandHandlerDecorator<>);
                services.Decorate(commandHandler, decorator.MakeGenericType(commandType));
            }
        }
    }
    
    private static void ConfigureCommandWithResultHandlersDecoratorsForValidation(this IServiceCollection services)
    {
        var assembly = typeof(Extensions).Assembly;
        var types = assembly.GetTypes();
        
        var validatableTypes = types
            .Where(x => x.BaseType?.IsGenericType == true && x.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>))
            .Select(x => x.BaseType!.GenericTypeArguments[0])
            .ToList();

        var commandWithResultHandlersTypes = types.Select(
                x => x.GetInterfaces().FirstOrDefault(@interface => @interface.IsGenericType
                && @interface.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)))
            .Where(x => x is not null).Cast<Type>().ToList();
        
        foreach (var commandHandler in commandWithResultHandlersTypes)
        {
            var commandType = commandHandler.GenericTypeArguments[0];
            var resultCommandType = commandHandler.GenericTypeArguments[1];
            if (validatableTypes.Contains(commandType))
            {
                var decorator = typeof(ValidationCommandHandlerDecorator<,>);
                services.Decorate(commandHandler, decorator.MakeGenericType(commandType, resultCommandType));
            }
        }
    }
}