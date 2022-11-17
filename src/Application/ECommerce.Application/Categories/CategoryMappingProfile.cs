using AutoMapper;
using ECommerce.Domain.Products;

namespace ECommerce.Application.Categories;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}