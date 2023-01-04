using ECommerce.Domain.Products.ValueObjects;
using ECommerce.Domain.Shared.Services;
using ECommerce.Domain.Shared.ValueObjects;

namespace ECommerce.Domain.Products;

public class Category
{
    public required CategoryId Id { get; init; }
    public required CategoryName Name { get; set; }
}