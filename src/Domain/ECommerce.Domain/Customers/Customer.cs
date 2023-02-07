using ECommerce.Domain.Customers.Entities;
using ECommerce.Domain.Customers.ValueObjects;
using ECommerce.Domain.Shared.ValueObjects;
using ECommerce.Domain.Shared.ValueObjects.Ids;

namespace ECommerce.Domain.Customers;

public class Customer
{
    public required CustomerId Id { get; init; }
    
    public required FirstName FirstName { get; set; }
    public required LastName LastName { get; set; }
    
    public required Email Email { get; set; }
    public PhoneNumber? PhoneNumber { get; set; }

    //public Cart Cart { get; set; }
    public List<Address> Addresses { get; }


    private Customer()
    {
        Addresses = new List<Address>();
    }
    
    public static Customer CreateRegistered(CustomerId customerId, FirstName firstName, LastName lastName, Email email, PhoneNumber? phoneNumber)
    {
        return new Customer()
        {
            Id = customerId,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PhoneNumber = phoneNumber,
        };
    }
}