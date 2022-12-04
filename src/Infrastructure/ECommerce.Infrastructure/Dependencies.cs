using System.Text;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Users.Interfaces;
using ECommerce.Application.Users.Models;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Identity.Settings;
using ECommerce.Infrastructure.Logging;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Seeders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using AuthorizationPolicy = ECommerce.Infrastructure.Identity.AuthorizationPolicy;

namespace ECommerce.Infrastructure;

public static class Dependencies
{
    public static void ConfigureInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        // setup configuration for SQL Server ECommerceDbContext
        services.AddDbContext<ECommerceDbContext>(options => options.UseSqlServer(connectionString));
        // seeder for ECommerceDbContext
        services.AddScoped<ECommerceDbSeeder>();
        // set (application layer) database
        services.AddScoped<IApplicationDatabase>(provider => provider.GetRequiredService<ECommerceDbContext>());

        services.ConfigureAuthentication(configuration);
        services.ConfigureAuthorization();
        
        // configure logging (Serilog)
        services.ConfigureSerilog();
    }
}