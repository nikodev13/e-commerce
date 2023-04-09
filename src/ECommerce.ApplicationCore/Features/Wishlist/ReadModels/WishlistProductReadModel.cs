using System.Xml;
using ECommerce.ApplicationCore.Entities;

namespace ECommerce.ApplicationCore.Features.Wishlist.ReadModels;

public record WishlistProductReadModel(long ProductId, string ProductName, bool IsAvailable)
{
    public static WishlistProductReadModel From(WishlistProduct product)
        => new(product.ProductId, product.Product.Name, product.Product is { InStockQuantity: > 0, IsActive: true });
}