using ECommerce.Domain.Products.Repositories;
using ECommerce.Infrastructure.Database;
using ECommerce.Infrastructure.Domain.Products.Repositories;
using ECommerce.Infrastructure.Domain.Products.Seeders;
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
        
        // registering repositories
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        
        return services;
    }
}