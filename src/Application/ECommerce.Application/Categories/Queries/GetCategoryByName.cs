using AutoMapper;
using ECommerce.Application.Shared.Results;
using ECommerce.Application.Shared.Results.Errors;
using ECommerce.Domain.Products.Repositories;
using MediatR;

namespace ECommerce.Application.Categories.Queries;

public class GetCategoryByNameQuery : IRequest<Result<CategoryDto>>
{
    public GetCategoryByNameQuery(string categoryName)
    {
        CategoryName = categoryName;
    }
    
    public string CategoryName { get; }
}

public class GetCategoryByNameQueryHandler : IRequestHandler<GetCategoryByNameQuery, Result<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoryByNameQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<Result<CategoryDto>> Handle(GetCategoryByNameQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByNameAsync(request.CategoryName);
        if (category is null) 
            return new NotFoundError($"Category with name {request.CategoryName} not found.");
            
        var result = _mapper.Map<CategoryDto>(category);
        return result;
    }
}

