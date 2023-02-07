using ECommerce.Domain.Users.Entities;

namespace ECommerce.Domain.Users.Abstractions;

public interface ITokenProvider
{
    public string GenerateAccessToken(User user);
    public string GenerateRefreshToken();
}