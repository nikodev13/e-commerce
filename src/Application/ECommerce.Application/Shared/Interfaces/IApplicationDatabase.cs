using ECommerce.Domain.Products;
using ECommerce.Domain.Products.Categories;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Shared.Interfaces;

public interface IApplicationDatabase
{
    DbSet<Category> Categories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}