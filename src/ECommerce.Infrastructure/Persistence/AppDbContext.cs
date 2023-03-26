using System.Reflection;
using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence;

public class AppDbContext : DbContext, IAppDbContext
{
    public DbSet<CustomerAccount> CustomersAccounts => Set<CustomerAccount>();
    public DbSet<CustomerAddress> Addresses => Set<CustomerAddress>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    
    public DbSet<User> Users => Set<User>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(AppDbContext))!);
        base.OnModelCreating(modelBuilder);
    }
}