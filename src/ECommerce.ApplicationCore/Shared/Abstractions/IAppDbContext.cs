using ECommerce.ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Shared.Abstractions;

public interface IAppDbContext
{
    DbSet<CustomerAccount> CustomersAccounts { get; }
    DbSet<CustomerAddress> Addresses { get; }
    DbSet<WishlistProduct> WishlistProducts { get; }
    DbSet<Order> Orders { get; }
    DbSet<OrderLine> OrderLines { get; }

    DbSet<Category> Categories { get; }
    DbSet<Product> Products { get; }
    
    DbSet<User> Users { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}