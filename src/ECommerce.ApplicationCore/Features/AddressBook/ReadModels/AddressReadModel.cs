using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.AddressBook.ReadModels;

public record AddressReadModel(string Street, string PostalCode, string City)
{
    public static AddressReadModel From(Address address)
        => new(address.Street, address.PostalCode, address.City);
}
