using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ECommerce.Application.Users.Interfaces;
using ECommerce.Application.Users.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ECommerce.Infrastructure.Authentication;

public class TokenProvider : ITokenProvider
{
    private readonly AuthenticationSettings _authenticationSettings;

    public TokenProvider(IOptions<AuthenticationSettings> jwtSettings)
    {
        _authenticationSettings = jwtSettings.Value;
    }
    
    public string GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        };
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.SecureKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(_authenticationSettings.AccessTokenExpireMinutes);

        var token = new JwtSecurityToken(
            issuer: _authenticationSettings.Issuer,
            audience:_authenticationSettings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: signingCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}