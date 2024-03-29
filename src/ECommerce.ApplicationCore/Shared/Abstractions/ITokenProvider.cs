using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Shared.Abstractions;

public interface ITokenProvider
{
    public string GenerateAccessToken(User user);
    public string GenerateRefreshToken();
}