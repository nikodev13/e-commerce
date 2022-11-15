using ECommerce.Domain.Products.Repositories;
using ECommerce.Domain.Products.Services;
using ECommerce.Domain.Products.ValueObjects;

namespace ECommerce.Application.Categories.DomainServices;

public class CategoryUniquenessChecker : ICategoryUniquenessChecker
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryUniquenessChecker(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<bool> IsUnique(CategoryName name)
    {
        return await _categoryRepository.GetByNameAsync(name) is null;
    }
}