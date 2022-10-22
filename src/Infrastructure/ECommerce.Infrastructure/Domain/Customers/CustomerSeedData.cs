using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Database;

namespace ECommerce.Infrastructure.Domain.Customers;

public class CustomerSeedData : IDatabaseSeedData<Customer>
{
    public ICollection<Customer> GetEntityData()
    {
        return new List<Customer>
        {
            new()
            {
                Id = new Guid(),
                Email = "example@gmail.com",
                PhoneNumber = "999888777",
                FirstName = "John",
                LastName = "Doe",
                FirstAddress = new Address()
                {
                    Id = new Guid(),
                    City = "Gdańsk",
                    Street = "al. Grunwaldzka 1",
                    PostalCode = "80-800"
                },
                Cart = new Cart()
                {
                    Id = Guid.NewGuid(),
                    Items = new List<CartItem>(),
                }
            }
        };
    }
}