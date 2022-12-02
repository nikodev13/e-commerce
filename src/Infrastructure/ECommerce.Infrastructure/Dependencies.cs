using System.Text;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Users.Interfaces;
using ECommerce.Infrastructure.Identity;
using ECommerce.Infrastructure.Identity.Settings;
using ECommerce.Infrastructure.Logging;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Infrastructure;

public static class Dependencies
{
    public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");
        services.ConfigureSerilog();
        services.AddDbContext<ECommerceDbContext>(options => options.UseSqlServer(connectionString));

        // seeder for ECommerceDbContext
        services.AddScoped<ECommerceDbSeeder>();
        // set application database
        services.AddScoped<IApplicationDatabase>(provider => provider.GetRequiredService<ECommerceDbContext>());
        // add authentication and authorization services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenProvider, JwtTokenProvider>();

        var jwtSettings = new JwtSettings();
            configuration.GetSection("JwtSettings").Bind(jwtSettings);        
        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = "Bearer";
            option.DefaultScheme = "Bearer";
            option.DefaultChallengeScheme = "Bearer";
        }).AddJwtBearer(cfg =>
        {
            cfg.RequireHttpsMetadata = false;
            cfg.SaveToken = true;
            cfg.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            };
        });
        services.AddAuthorization();
        
        return services;
    }
}