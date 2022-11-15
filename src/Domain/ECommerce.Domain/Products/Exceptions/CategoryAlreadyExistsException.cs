namespace ECommerce.Domain.Products.Exceptions;

public class CategoryAlreadyExistsException : Exception
{
    public CategoryAlreadyExistsException() : base("Category already exists.")
    {
        
    }
}