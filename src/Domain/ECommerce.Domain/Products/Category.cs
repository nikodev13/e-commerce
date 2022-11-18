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

    /// <summary>
    /// Creates a new unique instance of product category 
    /// </summary>
    /// <param name="categoryName">Name of product category</param>
    /// <param name="checker">Service checking that category already exists</param>
    /// <returns>A new instance of product category</returns>
    /// <exception cref="CategoryAlreadyExistsException">Throws when category already exists</exception>
    /// 
    public static async Task<Category> Create(CategoryName categoryName, ICategoryUniquenessChecker checker)
    {
        var isUnique = await checker.IsUnique(categoryName);
        
        if (!isUnique)
        {
            throw new CategoryAlreadyExistsException();
        }

        return new Category(categoryName);
    }
}