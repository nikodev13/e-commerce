using AutoMapper;
using ECommerce.Application.Shared.Interfaces;
using ECommerce.Application.Shared.Results;
using ECommerce.Application.Shared.Results.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Products.Categories.Features.Queries;

public class GetCategoryByIdQuery : IRequest<Result<CategoryDto>>
{
    public GetCategoryByIdQuery(long categoryId)
    {
        CategoryId = categoryId;
    }
    
    public long CategoryId { get; }
}

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
{
    private readonly IApplicationDatabase _database;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(IApplicationDatabase database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }
    
    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _database.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (category is null) 
            return new NotFoundError($"Category with ID {request.CategoryId} not found.");
            
        var result = _mapper.Map<CategoryDto>(category);
        return result;
    }
}