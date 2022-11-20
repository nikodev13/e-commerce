using AutoMapper;
using ECommerce.Domain.Products.Categories;

namespace ECommerce.Application.Products.Categories;

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