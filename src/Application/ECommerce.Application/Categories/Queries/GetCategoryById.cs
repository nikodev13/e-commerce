using AutoMapper;
using ECommerce.Application.Shared.Results;
using ECommerce.Application.Shared.Results.Errors;
using ECommerce.Domain.Products.Repositories;
using MediatR;

namespace ECommerce.Application.Categories.Queries;

public class GetCategoryByIdQuery : IRequest<Result<CategoryDto>>
{
    public GetCategoryByIdQuery(Guid categoryId)
    {
        CategoryId = categoryId;
    }
    
    public Guid CategoryId { get; }
}

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }
    
    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
        if (category is null) 
            return new NotFoundError($"Category with ID {request.CategoryId} not found.");
            
        var result = _mapper.Map<CategoryDto>(category);
        return result;
    }
}