namespace ECommerce.Domain.Products.Categories.Exceptions;

public class CategoryAlreadyExistsException : Exception
{
    public CategoryAlreadyExistsException() : base("Category already exists.")
    {
        
    }
}