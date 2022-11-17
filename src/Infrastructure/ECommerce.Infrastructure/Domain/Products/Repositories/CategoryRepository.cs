using ECommerce.Domain.Products;
using ECommerce.Domain.Products.Repositories;
using ECommerce.Domain.Products.ValueObjects;
using ECommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Domain.Products.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ECommerceDbContext _dbContext;

    public CategoryRepository(ECommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Task<List<Category>> GetAllAsync()
    {
        return _dbContext.Categories.AsNoTracking().ToListAsync();
    }

    public Task<Category?> GetByIdAsync(CategoryId id)
    {
        return _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Value == id.Value);
    }

    public Task<Category?> GetByNameAsync(CategoryName name)
    {
        return _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Name.Value == name.Value);
    }

    public async Task AddAsync(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Category category)
    {
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category category)
    {
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
    }
}