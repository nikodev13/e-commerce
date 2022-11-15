using ECommerce.Domain.Products.Exceptions;
using ECommerce.Domain.Products.Services;
using ECommerce.Domain.Products.ValueObjects;

namespace ECommerce.Domain.Products;

public class Category
{
    public CategoryId Id { get; }
    public CategoryName Name { get; set; }

    private Category()
    {
        Id = new CategoryId(Guid.NewGuid());
    }
    
    private Category(CategoryName name) : this()
    {
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