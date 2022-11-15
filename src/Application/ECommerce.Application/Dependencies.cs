using System.Reflection;
using ECommerce.Application.Categories.DomainServices;
using ECommerce.Domain.Products.Services;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace ECommerce.Application;

public static class Dependencies
{
    public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddScoped<ICategoryUniquenessChecker, CategoryUniquenessChecker>();
                    
        return services;
    }
}