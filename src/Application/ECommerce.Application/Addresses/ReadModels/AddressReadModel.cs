using ECommerce.Domain.Addresses;

namespace ECommerce.Application.Addresses.ReadModels;

public class AddressReadModel
{
    public required long Id { get; init; }
    public required string Street { get; init; }
    public required string PostalCode { get; init; }
    public required string City { get; init; }

    public static AddressReadModel FromAddress(Address address)
    {
        return new AddressReadModel
        {
            Id = address.Id.Value,
            Street = address.Street.Value,
            PostalCode = address.PostalCode.Value,
            City = address.City.Value,
        };
    } 
}