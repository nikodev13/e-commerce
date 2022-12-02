using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Common.Results;
using ECommerce.Application.Common.Results.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.ProductCategories.Queries;

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

    public GetCategoryByIdQueryHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _database.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (category is null) 
            return new NotFoundError($"Category with ID {request.CategoryId} not found.");
            
        var result = CategoryDto.FromCategory(category);
        return result;
    }
}