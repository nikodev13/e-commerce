using Bogus;
using ECommerce.Application.Common.Services;
using ECommerce.Domain.Customers;
using ECommerce.Domain.Customers.ValueObjects;
using ECommerce.Domain.Shared.Services;

namespace ECommerce.Infrastructure.Persistence.Seeders;

internal static class CustomersSeedDataProvider
{
    private static ISnowflakeIdService _idService;
    private static ApplicationDbContext _dbContext;

    public static void SeedCustomersContextSampleData(this ApplicationDbContext dbContext)
    {
        _idService = new SnowflakeIdService();
        _dbContext = dbContext;
        if (!dbContext.Customers.Any())
        {
            var data = GetCustomersData();
            dbContext.Customers.AddRange(data);
            dbContext.SaveChanges();
        }
    }
    
    private static IEnumerable<Customer> GetCustomersData()
    {
        var fakeCustomerFactory = new Faker<Customer>()
            .CustomInstantiator(x => 
                Customer.CreateRegistered(Guid.NewGuid(),
                    new FirstName(x.Name.FirstName()),
                    new LastName(x.Name.LastName()),
                    new Email(x.Internet.ExampleEmail()), 
                    new PhoneNumber(x.Phone.PhoneNumber())));
    
        var data = fakeCustomerFactory.Generate(10);
        return data;
    
    }
}