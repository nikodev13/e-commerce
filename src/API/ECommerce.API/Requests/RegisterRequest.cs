namespace ECommerce.API.Requests;

public class RegisterRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}