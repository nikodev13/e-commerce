using ECommerce.Domain.Products.ValueObjects;

namespace ECommerce.Domain.Products.Repositories;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(CategoryId id);
    Task<Category?> GetByNameAsync(CategoryName name);
    Task AddAsync(Category category);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Category category);
}