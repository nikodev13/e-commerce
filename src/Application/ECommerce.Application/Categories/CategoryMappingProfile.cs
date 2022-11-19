using AutoMapper;
using ECommerce.Domain.Products;
using ECommerce.Domain.ProductsContext;

namespace ECommerce.Application.Categories;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ForMember(x => x.Id,
                x => x.MapFrom(y => y.Id.Value))
            .ForMember(x => x.Name, 
                x => x.MapFrom(x => x.Name.Value));
    }
}