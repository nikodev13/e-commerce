using System.Reflection;
using ECommerce.ApplicationCore.Features.Users.Commands;
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
        services.ConfigureCqrs();
        services.AddValidatorsFromAssemblyContaining(typeof(Extensions), ServiceLifetime.Scoped, null, true);
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        
        return services;
    }
}