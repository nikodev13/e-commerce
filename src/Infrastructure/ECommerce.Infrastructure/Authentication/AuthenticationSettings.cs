namespace ECommerce.Infrastructure.Authentication;

public class AuthenticationSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string SecureKey { get; set; }
    public int AccessTokenExpireMinutes { get; set; } = 30;

    public int RefreshTokenExpireMinutes { get; set; } = 1440;
}