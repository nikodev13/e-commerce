namespace ECommerce.ApplicationCore.Entities;

public class CustomerAccount
{
    public required Guid Id { get; init; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }

    public List<CustomerAddress> Addresses { get; } = default!;
}

public class CustomerAddress
{
    public required long Id { get; init; }
    public required Guid CustomerId { get; init; }
    public required string Street { get; set; }
    public required string PostalCode { get; set; }
    public required string City { get; set; }
}