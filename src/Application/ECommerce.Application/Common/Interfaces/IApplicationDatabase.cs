using ECommerce.Application.Users.Models;
using ECommerce.Domain.Addresses;
using ECommerce.Domain.Customers;
using ECommerce.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Common.Interfaces;

public interface IApplicationDatabase
{
    DbSet<Category> Categories { get; }
    DbSet<Product> Products { get; }
    
    
    DbSet<Customer> Customers { get; }
    DbSet<Address> Addresses { get; }

    
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}