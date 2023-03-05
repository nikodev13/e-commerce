using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Customers.Adresses.Exceptions;

public class AddressNotFoundException : NotFoundException
{
    public AddressNotFoundException(long id) : base($"Address with id `{id}` not found for current user.") { }
}