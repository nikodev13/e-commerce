using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.ProductCategories.Features.Queries;

public class GetCategoryByIdQuery : IRequest<CategoryDto>
{
    public GetCategoryByIdQuery(long categoryId)
    {
        CategoryId = categoryId;
    }
    
    public long CategoryId { get; }
}

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly IApplicationDatabase _database;

    public GetCategoryByIdQueryHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _database.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == request.CategoryId, cancellationToken);
        if (category is null) 
            throw new NotFoundException($"Category with ID {request.CategoryId} not found.");
            
        var result = CategoryDto.FromCategory(category);
        return result;
    }
}