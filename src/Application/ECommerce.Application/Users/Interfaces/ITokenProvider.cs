using ECommerce.Application.Users.Models;

namespace ECommerce.Application.Users.Interfaces;

public interface ITokenProvider
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken(User user);
}