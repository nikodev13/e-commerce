using ECommerce.Application.Users.Models;
using ECommerce.Domain.Products;
using ECommerce.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Common.Interfaces;

public interface IApplicationDatabase
{
    DbSet<Category> Categories { get; }
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    DbSet<TEntity> Set<TEntity>() where TEntity : Entity;
}