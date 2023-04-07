namespace ECommerce.ApplicationCore.Entities;

public class Customer
{
    public required Guid Id { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public DateTime RegisteredAt { get; } = DateTime.Now;
}


