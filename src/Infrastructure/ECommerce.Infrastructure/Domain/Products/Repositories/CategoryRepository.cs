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
        return _dbContext.Categories.ToListAsync();
    }

    public Task<Category> GetByIdAsync(CategoryId id)
    {
        throw new NotImplementedException();
    }

    public Task<Category?> GetByNameAsync(CategoryName name)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Category category)
    {
        throw new NotImplementedException();
    }

    public Task Update(Category category)
    {
        throw new NotImplementedException();
    }
}