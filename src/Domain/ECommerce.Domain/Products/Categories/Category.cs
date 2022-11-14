using ECommerce.Domain.Products.Categories.Exceptions;
using ECommerce.Domain.Products.Categories.ValueObjects;

namespace ECommerce.Domain.Products.Categories;

public class Category
{
    public CategoryId Id { get; }
    public CategoryName Name { get; }

    private Category(CategoryName name)
    {
        Id = new CategoryId(0);
        Name = name;
    }

    public static async Task<Category> Create(CategoryName categoryName, ICategoryUniquenessChecker checker)
    {
        var categoryAlreadyExists = await checker.IsUnique(categoryName);
        
        if (categoryAlreadyExists)
        {
            throw new CategoryAlreadyExistsException();
        }

        return new Category(categoryName);
    }
}