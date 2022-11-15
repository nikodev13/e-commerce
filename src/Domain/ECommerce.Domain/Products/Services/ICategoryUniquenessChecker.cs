using ECommerce.Domain.Products.ValueObjects;

namespace ECommerce.Domain.Products.Services;

public interface ICategoryUniquenessChecker
{
    Task<bool> IsUnique(CategoryName name);
}