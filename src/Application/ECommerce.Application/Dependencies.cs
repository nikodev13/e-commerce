using System.Reflection;
using ECommerce.Application.Products.Categories;
using ECommerce.Application.Products.Categories.Services;
using ECommerce.Application.Shared.Services;
using ECommerce.Domain.Products.Categories;
using ECommerce.Domain.Products.Categories.Services;
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
        services.AddScoped<ICategoryUniquenessChecker, CategoryUniquenessChecker>();
                    
        return services;
    }
}