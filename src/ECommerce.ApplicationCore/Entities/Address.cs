namespace ECommerce.ApplicationCore.Entities;

public class Address
{
    public required long Id { get; init; }
    public required Guid CustomerId { get; init; }
    public required string Street { get; set; }
    public required string PostalCode { get; set; }
    public required string City { get; set; }
}