using System.Text;
using ECommerce.Application.Users.Interfaces;
using ECommerce.Application.Users.Models;
using ECommerce.Infrastructure.Identity.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Infrastructure.Identity;

public static class IdentityConfigurations
{
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // add (application layer) authentication
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenProvider, JwtTokenProvider>();
        
        // set jwt settings from appsettings.json
        var jwtSettings = new JwtSettings();
        configuration.GetSection("JwtSettings").Bind(jwtSettings);       
        
        // configure authentication
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
    }
    
    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthorizationPolicy.Admin, policy => policy.RequireRole(nameof(Role.Admin)));
        });
    }
}