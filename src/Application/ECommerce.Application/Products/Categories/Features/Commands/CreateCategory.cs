using AutoMapper;
using ECommerce.Application.Shared.Interfaces;
using ECommerce.Application.Shared.Results;
using ECommerce.Application.Shared.Results.Errors;
using ECommerce.Domain.Products.Categories;
using ECommerce.Domain.Products.Categories.Exceptions;
using ECommerce.Domain.Products.Categories.Services;
using ECommerce.Domain.Shared.Services;
using MediatR;

namespace ECommerce.Application.Products.Categories.Features.Commands;

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
    private readonly ICategoryUniquenessChecker _uniquenessChecker;
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(IApplicationDatabase database,
        ISnowflakeIdService idService,
        ICategoryUniquenessChecker uniquenessChecker,
        IMapper mapper)
    {
        _database = database;
        _idService = idService;
        _uniquenessChecker = uniquenessChecker;
        _mapper = mapper;
    }
    
    public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category newCategory;
        try
        {
            newCategory = Category.Create(request.CategoryName, _idService, _uniquenessChecker);
        }
        catch (CategoryAlreadyExistsException exception)
        {
            return new AlreadyExistsError(exception.Message);
        }
        await _database.Categories.AddAsync(newCategory, cancellationToken);
        await _database.SaveChangesAsync(cancellationToken);
        var result = _mapper.Map<CategoryDto>(newCategory);
        
        return result;
    }
}