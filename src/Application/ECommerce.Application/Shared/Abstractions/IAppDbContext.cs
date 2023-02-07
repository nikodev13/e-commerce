using ECommerce.Domain.Management.Entities;
using ECommerce.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Shared.Abstractions;

public interface IAppDbContext
{
    DbSet<Category> Categories { get; }
    DbSet<Product> Products { get; }
    
    DbSet<User> Users { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}