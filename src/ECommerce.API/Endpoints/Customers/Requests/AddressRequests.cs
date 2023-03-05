namespace ECommerce.API.Endpoints.Customers.Requests;

public class AddNewAddressRequest
{
    public required string Street { get; init; }
    public required string PostalCode { get; init; }
    public required string City { get; init; }
}

public class UpdateAddressRequest
{
    public required string Street { get; init; }
    public required string PostalCode { get; init; }
    public required string City { get; init; }
}