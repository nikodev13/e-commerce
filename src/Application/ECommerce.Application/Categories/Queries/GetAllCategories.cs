using AutoMapper;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Categories.Queries;

public class GetAllCategoriesQuery : IRequest<Result<List<CategoryDto>>> 
{
}

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Result<List<CategoryDto>>>
{
    private readonly IApplicationDatabase _database;
    private readonly IMapper _mapper;

    public GetAllCategoriesQueryHandler(IApplicationDatabase database, IMapper mapper)
    {
        _database = database;
        _mapper = mapper;
    }
    
    public async Task<Result<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _database.Categories.AsNoTracking().ToListAsync(cancellationToken);
        var result = _mapper.Map<List<CategoryDto>>(categories);
        return result;
    }
}
