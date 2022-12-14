using System.IdentityModel.Tokens.Jwt;
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
        services.Configure<AuthenticationSettings>(configuration.GetSection("AuthenticationSettings"));
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        
        // set jwt settings from appsettings.json
        var jwtSettings = new AuthenticationSettings();
        configuration.GetSection("AuthenticationSettings").Bind(jwtSettings);       
        
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
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecureKey)),
            };
        });
        
        // for mapping real claims types (i.a. sub not NameIdentifier)
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
    }
}