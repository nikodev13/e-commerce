using ECommerce.Domain.Addresses.ValueObjects;
using ECommerce.Domain.Customers.ValueObjects;
using ECommerce.Domain.Shared.Exceptions;
using ECommerce.Domain.Shared.Services;

namespace ECommerce.Domain.Addresses;

public class Address
{
    public required AddressId Id { get; init; }
    public required CustomerId CustomerId { get; init; }

    public required Street Street { get; set; }
    public required PostalCode PostalCode { get; set; }
    public required City City { get; set; }
    private Address() {}

    public static Address CreateNew(CustomerId customerId, Street street, PostalCode postalCode, City city, ISnowflakeIdService idService)
    {
        return new Address()
        {
            Id = idService.GenerateId(),
            CustomerId = customerId,
            Street = street,
            PostalCode = postalCode,
            City = city
        };
    }
}