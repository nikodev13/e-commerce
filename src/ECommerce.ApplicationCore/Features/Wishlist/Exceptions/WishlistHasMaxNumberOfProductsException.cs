using ECommerce.ApplicationCore.Shared.Exceptions;

namespace ECommerce.ApplicationCore.Features.Wishlist.Exceptions;

public class WishlistHasMaxNumberOfProductsException : BadRequestException
{
    public WishlistHasMaxNumberOfProductsException() : base("Wishlist has max number of products.") { }
}