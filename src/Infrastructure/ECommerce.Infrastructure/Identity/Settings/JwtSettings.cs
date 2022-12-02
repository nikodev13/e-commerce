namespace ECommerce.Infrastructure.Identity.Settings;

public class JwtSettings
{
    public string JwtKey { get; set; }
    public int JwtExpireMinutes { get; set; }
    public string JwtIssuer { get; set; }
}