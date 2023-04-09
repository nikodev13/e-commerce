using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Wishlist.Exceptions;

public class ProductNotFoundInWishlistException : BadRequestException
{
    public ProductNotFoundInWishlistException(long productId) : base($"Product with id `{productId}` not found in") { }
}