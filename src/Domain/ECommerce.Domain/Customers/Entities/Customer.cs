using ECommerce.Domain.Customers.ValueObjects;
using ECommerce.Domain.Shared.Services;
using ECommerce.Domain.Shared.ValueObjects;
using ECommerce.Domain.Shared.ValueObjects.Ids;

namespace ECommerce.Domain.Customers.Entities;

public class Customer
{
    public required CustomerId Id { get; init; }

    private FirstName _firstName = default!;
    private LastName _lastName = default!;
    private Email _email = default!;
    private PhoneNumber _phoneNumber = default!;
    
    
    private Customer() { }

    public static Customer CreateRegistered()
    {
        return new Customer
        {
            Id = Guid.NewGuid()
        };
    } 
}