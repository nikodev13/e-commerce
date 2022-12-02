namespace ECommerce.Infrastructure.Identity.Settings;

public class JwtSettings
{
    public string Key { get; set; }
    public int ExpireMinutes { get; set; }
    public string Issuer { get; set; }
}