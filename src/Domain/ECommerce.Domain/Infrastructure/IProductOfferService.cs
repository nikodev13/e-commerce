using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Infrastructure;

public interface IProductOfferRepository
{
    Task<ProductOffer> GetProductOfferById(Guid id);
    Task<ICollection<ProductOffer>> GetAllProductOffer();
}