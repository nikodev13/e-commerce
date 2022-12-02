using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECommerce.Application.Users.Interfaces;
using ECommerce.Application.Users.Models;
using ECommerce.Infrastructure.Identity.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Infrastructure.Identity;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenProvider(IConfiguration configuration)
    {
        _jwtSettings = new JwtSettings();
        configuration.GetSection("JwtSettings").Bind(_jwtSettings);       
    }
    
    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(_jwtSettings.ExpireMinutes);

        var token = new JwtSecurityToken(_jwtSettings.Issuer,
            _jwtSettings.Issuer,
            claims,
            expires: expires,
            signingCredentials: cred);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken(User user)
    {
        throw new NotImplementedException();
    }
}