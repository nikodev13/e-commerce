using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Orders.Exceptions;

public class PlaceOrderException : BadRequestException
{
    public PlaceOrderException(string message) : base(message) { }
}