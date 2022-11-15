using ECommerce.Domain.Shared.ValueObjects;

namespace ECommerce.Domain.Products.Repositories;

public interface IProductRepository
{
    Task<Product> GetById(ProductId id);
}