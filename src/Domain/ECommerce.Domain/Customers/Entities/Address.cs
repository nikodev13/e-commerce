using ECommerce.Domain.Customers.ValueObjects;
using ECommerce.Domain.Shared.Services;
using ECommerce.Domain.Shared.ValueObjects;
using ECommerce.Domain.Shared.ValueObjects.Ids;

namespace ECommerce.Domain.Customers.Entities;

public class Address
{
    public required AddressId Id { get; init; }
    public required CustomerId CustomerId { get; init; }

    public required Street Street { get; set; }
    public required PostalCode PostalCode { get; set; }
    public required City City { get; set; }
    private Address() {}

    public static Address CreateNew(CustomerId customerId, Street street, PostalCode postalCode, City city, ISnowflakeIdProvider idProvider)
    {
        return new Address()
        {
            Id = idProvider.GenerateId(),
            CustomerId = customerId,
            Street = street,
            PostalCode = postalCode,
            City = city
        };
    }
}