using System.Reflection;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Users.Models;
using ECommerce.Domain.Products;
using ECommerce.Domain.SeedWork;
using ECommerce.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Tests.Persistence;

public sealed class AppDbContext : DbContext, IApplicationDatabase
{
    public DbSet<Category> Categories => base.Set<Category>();
    public DbSet<Product> Products => base.Set<Product>();
    public DbSet<User> Users => base.Set<User>();

    public new DbSet<TEntity> Set<TEntity>() where TEntity : Entity
    {
        return base.Set<TEntity>();
    }

    public AppDbContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var path = "Filename=e-commercetestdb.db3";
        optionsBuilder.UseSqlite(path);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(CategoryEntityTypeConfiguration)));
    }
}