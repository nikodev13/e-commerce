using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.CustomerAccounts.Exceptions;

public class MoreThanThreeAddressesException : BadRequestException
{
    public MoreThanThreeAddressesException() : base("The maximum number of addresses is three.") { }
}