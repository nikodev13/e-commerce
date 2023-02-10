using System.Reflection;
using ECommerce.Application.Shared.CQRS;
using ECommerce.Domain;
using ECommerce.Domain.Shared.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Application;

public static class Extensions
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.RegisterApplicationHandlers();
        services.AddValidatorsFromAssembly(assembly);
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        services.AddSingleton<ISnowflakeIdProvider, SnowflakeIdProvider>();
        return services;
    }
}