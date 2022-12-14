using ECommerce.Application.Common.Interfaces;
using ECommerce.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Infrastructure.Persistence;

internal static class Configuration
{
    public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<ApplicationDbSeeder>();
        services.AddScoped<IApplicationDatabase>(provider => provider.GetRequiredService<ApplicationDbContext>());
    }
}