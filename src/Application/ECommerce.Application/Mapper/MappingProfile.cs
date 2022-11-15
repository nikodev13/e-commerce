using AutoMapper;
using ECommerce.Application.Categories;
using ECommerce.Domain.Products;

namespace ECommerce.Application.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category,CategoryDto>().ReverseMap();
    }
}