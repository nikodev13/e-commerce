using System.Reflection;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Products;
using ECommerce.Infrastructure.Domain.Customers;
using ECommerce.Infrastructure.Domain.Products.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Database;

public class ECommerceDbContext : DbContext
{
    //public DbSet<Customer> Customers { get; set; }
    //public DbSet<Cart> Carts { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
}