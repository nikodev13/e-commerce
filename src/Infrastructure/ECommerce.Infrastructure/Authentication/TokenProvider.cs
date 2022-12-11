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
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.SecureKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(_authenticationSettings.AccessTokenExpireMinutes);

        var token = new JwtSecurityToken(_authenticationSettings.Issuer,
            _authenticationSettings.Issuer,
            claims,
            expires: expires,
            signingCredentials: signingCredentials);

        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumbers = RandomNumberGenerator.GetBytes(32);
        return Encoding.ASCII.GetString(randomNumbers);
    }
}