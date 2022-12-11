using System.Text;
using ECommerce.Application.Users.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Infrastructure.Authentication;

internal static class Configuration
{
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // add (application layer) authentication
        services.Configure<AuthenticationSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        
        // set jwt settings from appsettings.json
        var jwtSettings = new AuthenticationSettings();
        configuration.GetSection("JwtSettings").Bind(jwtSettings);       
        
        // configure authentication
        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = "Bearer";
            option.DefaultScheme = "Bearer";
            option.DefaultChallengeScheme = "Bearer";
        }).AddJwtBearer(config =>
        {
            config.RequireHttpsMetadata = false;
            config.SaveToken = true;
            config.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecureKey)),
            };
        });
    }
}