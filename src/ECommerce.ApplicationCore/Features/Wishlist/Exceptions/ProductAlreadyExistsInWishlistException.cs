using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Wishlist.Exceptions;

public class ProductAlreadyExistsInWishlistException : BadRequestException
{
    public ProductAlreadyExistsInWishlistException(long productId) : base($"Product with id '{productId}' already exists in wishlist.") { }
}