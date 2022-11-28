using System.Reflection;
using ECommerce.Application.Common.Behaviours;
using ECommerce.Application.Common.Services;
using ECommerce.Domain.Shared.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace ECommerce.Application;

public static class Dependencies
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        services.AddMediatR(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddValidatorsFromAssembly(assembly);
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        // services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddSingleton<ISnowflakeIdService, SnowflakeIdService>();
                    
        return services;
    }
}