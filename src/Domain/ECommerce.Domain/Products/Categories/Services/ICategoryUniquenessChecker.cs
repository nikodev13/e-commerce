namespace ECommerce.Domain.Products.Categories.Services;

public interface ICategoryUniquenessChecker
{
    bool IsNotUnique(CategoryName name);
}