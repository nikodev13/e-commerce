using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Customers.Adresses.Exceptions;

public class MoreThanThreeAddressesException : BadRequestException
{
    public MoreThanThreeAddressesException() : base("The maximum number of addresses is three.") { }
}