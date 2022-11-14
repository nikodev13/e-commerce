using ECommerce.Domain.Products.Categories.ValueObjects;

namespace ECommerce.Domain.Products.Categories;

public interface ICategoryUniquenessChecker
{
    Task<bool> IsUnique(CategoryName name);
}