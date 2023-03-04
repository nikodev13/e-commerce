using System.Reflection;
using ECommerce.ApplicationCore.Shared;
using ECommerce.ApplicationCore.Shared.CQRS;
using ECommerce.ApplicationCore.Shared.Abstractions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.ApplicationCore;

public static class Extensions
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.ConfigureCqrs();
        services.AddValidatorsFromAssembly(assembly);
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        
        return services;
    }
}