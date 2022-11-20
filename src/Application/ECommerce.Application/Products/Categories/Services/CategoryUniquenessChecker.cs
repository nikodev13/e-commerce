using ECommerce.Application.Shared.Interfaces;
using ECommerce.Domain.Products.Categories;
using ECommerce.Domain.Products.Categories.Services;

namespace ECommerce.Application.Products.Categories.Services;

public class CategoryUniquenessChecker : ICategoryUniquenessChecker
{
    private readonly IApplicationDatabase _database;

    public CategoryUniquenessChecker(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public bool IsNotUnique(CategoryName name)
    {
        return _database.Categories.Any(x => x.Name == name);
    }
}