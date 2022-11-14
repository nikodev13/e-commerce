using ECommerce.Domain.Products.Categories.ValueObjects;

namespace ECommerce.Domain.Products.Categories;

public interface ICategoryRepository
{
    Task<Category> GetByIdAsync(CategoryId id);
    Task<Category> GetByNameAsync(CategoryName name);
    Task AddAsync(Category category);
    Task Update(Category category);
}