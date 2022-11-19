using AutoMapper;
using ECommerce.Application.Interfaces;
using ECommerce.Application.Shared.Results;
using ECommerce.Application.Shared.Results.Errors;
using ECommerce.Domain.Products;
using ECommerce.Domain.Shared.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Categories.Commands;

public class CreateCategoryCommand : IRequest<Result<CategoryDto>>
{
    public CreateCategoryCommand(string categoryName)
    {
        CategoryName = categoryName;
    }
    
    public string CategoryName { get; }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
{
    private readonly IApplicationDatabase _database;
    private readonly ISnowflakeIdService _idService;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(IApplicationDatabase database, ISnowflakeIdService idService, IMapper mapper)
    {
        _database = database;
        _idService = idService;
        _mapper = mapper;
    }
    
    public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (await _database.Categories.AnyAsync(c => c.Name == request.CategoryName, cancellationToken))
        {
            return new AlreadyExistsError($"Category with name {request.CategoryName} already exists.");
        }

        var category = Category.Create(request.CategoryName, _idService);
        await _database.Categories.AddAsync(category, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);
        
        var result = _mapper.Map<CategoryDto>(category);
        return result;
    }
}