using ECommerce.Application.Interfaces;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure;

public static class Dependencies
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddDbContext<ECommerceDbContext>(options => options.UseSqlServer(connectionString));

        // seeder for ECommerceDbContext
        services.AddScoped<ECommerceDbSeeder>();
        services.AddScoped<IApplicationDatabase>(provider => provider.GetRequiredService<ECommerceDbContext>());
        
        return services;
    }
}