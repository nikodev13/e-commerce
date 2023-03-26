using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.CustomerAccounts.ReadModels;

public record AddressReadModel(long Id, string Street, string PostalCode, string City)
{
    public static AddressReadModel From(CustomerAddress address) 
        => new(address.Id, address.Street, address.PostalCode, address.City);
}
