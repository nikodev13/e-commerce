using ECommerce.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Shared.Abstractions;

public interface IAppDbContext
{
    DbSet<CustomerAccount> CustomersAccounts { get; }
    DbSet<Address> Addresses { get; }
    DbSet<Order> Orders { get; }

    
    DbSet<Category> Categories { get; }
    DbSet<Product> Products { get; }
    
    DbSet<User> Users { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}