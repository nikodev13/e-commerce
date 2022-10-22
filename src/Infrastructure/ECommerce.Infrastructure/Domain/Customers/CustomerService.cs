using ECommerce.Domain.Entities;
using ECommerce.Domain.Infrastructure;
using ECommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Domain.Customers;

public class CustomerService : ICustomerService
{
    private readonly ECommerceDbContext _dbContext;

    public CustomerService(ECommerceDbContext _dbContext)
    {
        this._dbContext = _dbContext;
    }
    
    public async Task<Customer?> GetById(Guid id)
    {
        var customer = await _dbContext.Customers
            .FirstOrDefaultAsync(customer => customer.Id == id);
        return customer;
    }
}