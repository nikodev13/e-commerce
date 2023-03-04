using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.Customer.Adresses;

public class AddressReadModel
{
    public required long Id { get; init; }
    public required string Street { get; init; }
    public required string PostalCode { get; init; }
    public required string City { get; init; }
    
    private AddressReadModel() { }

    public static AddressReadModel From(Address address)
    {
        return new AddressReadModel
        {
            Id = address.Id,
            Street = address.Street,
            PostalCode = address.PostalCode,
            City = address.City
        };
    }
}