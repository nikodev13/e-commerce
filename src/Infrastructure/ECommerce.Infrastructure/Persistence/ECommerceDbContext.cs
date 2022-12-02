using System.Reflection;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Users.Models;
using ECommerce.Domain.Products;
using ECommerce.Domain.ProductsContext;
using ECommerce.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Persistence;

public class ECommerceDbContext : DbContext, IApplicationDatabase
{
    // for authentication and authorization
    public DbSet<User> Users => base.Set<User>();
    //
    public DbSet<Product> Products => base.Set<Product>();
    public DbSet<Category> Categories => base.Set<Category>();
    public new DbSet<TEntity> Set<TEntity>() where TEntity : Entity
    {
        return base.Set<TEntity>();
    }

    public ECommerceDbContext(DbContextOptions<ECommerceDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}