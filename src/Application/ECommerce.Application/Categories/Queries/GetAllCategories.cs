using AutoMapper;
using ECommerce.Application.Shared.Results;
using ECommerce.Domain.Products.Repositories;
using MediatR;

namespace ECommerce.Application.Categories.Queries;

public class GetAllCategoriesQuery : IRequest<Result<List<CategoryDto>>> 
{
}

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Result<List<CategoryDto>>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetAllCategoriesQueryHandler(ICategoryRepository repository, IMapper mapper)
    {
        _categoryRepository = repository;
        _mapper = mapper;
    }
    
    public async Task<Result<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync();
        var result = _mapper.Map<List<CategoryDto>>(categories);
        return result;
    }
}
