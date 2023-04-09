using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.AddressBook.ReadModels;

namespace ECommerce.ApplicationCore.Features.Customers.ReadModels;

public record CustomerAccountReadModel(string FirstName, string LastName, string Email, string PhoneNumber)
{
    public static CustomerAccountReadModel From(Customer account)
        => new(account.FirstName,
            account.LastName,
            account.Email,
            account.PhoneNumber);
}