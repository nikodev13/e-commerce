using ECommerce.Application.Tests.Users;
using ECommerce.ApplicationCore.Entities;

namespace ECommerce.Application.Tests.Customers;

public static class DummyCustomersAccounts
{
    public static List<Customer> Data = new()
    {
        new Customer
        {
            Id = DummyUsers.Data[0].Id,
            FirstName = "John",
            LastName = "Doe",
            Email = DummyUsers.Data[0].Email,
            PhoneNumber = "111222333"
        }
    };
}