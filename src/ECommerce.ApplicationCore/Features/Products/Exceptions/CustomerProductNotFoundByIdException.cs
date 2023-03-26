using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Products.Exceptions;

public class CustomerProductNotFoundByIdException : NotFoundException
{
    public CustomerProductNotFoundByIdException(long id) : base($"Product with id `{id}` not found.") { }
}