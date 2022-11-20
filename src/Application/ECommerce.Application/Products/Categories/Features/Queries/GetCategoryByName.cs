using AutoMapper;
using ECommerce.Application.Shared.Interfaces;
using ECommerce.Application.Shared.Results;
using ECommerce.Application.Shared.Results.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Products.Categories.Features.Queries;

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
    private readonly IApplicationDatabase _database;
    private readonly IMapper _mapper;

    public GetCategoryByNameQueryHandler(IApplicationDatabase database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }
    
    public async Task<Result<CategoryDto>> Handle(GetCategoryByNameQuery request, CancellationToken cancellationToken)
    {
        var category = await _database.Categories.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == request.CategoryName, cancellationToken);
        
        if (category is null) 
            return new NotFoundError($"Category with name {request.CategoryName} not found.");
            
        var result = _mapper.Map<CategoryDto>(category);
        return result;
    }
}

