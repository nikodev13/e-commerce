using ECommerce.Domain.Products;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Interfaces;

public interface IApplicationDatabase
{
    DbSet<Category> Categories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}