using ECommerce.Domain.Shared.ValueObjects;

namespace ECommerce.Domain.Products;

public interface IProductRepository
{
    Task<Product> GetById(ProductId id);
}