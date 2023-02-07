using ECommerce.Application.Management.Categories.Exceptions;
using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Shared.CQRS;
using ECommerce.Domain.Management.Entities;
using ECommerce.Domain.Shared.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Management.Categories.Commands;

public class CreateCategoryCommand : ICommand<CategoryReadModel>
{
    public required string Name { get; init; }
}

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryReadModel>
{
    private readonly IAppDbContext _dbContext;
    private readonly ISnowflakeIdProvider _idProvider;
    private readonly IUserContextProvider _userContextProvider;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    public CreateCategoryCommandHandler(IAppDbContext dbContext,
        ISnowflakeIdProvider idProvider,
        IUserContextProvider userContextProvider,
        ILogger<CreateCategoryCommandHandler> logger)
    {
        _dbContext = dbContext;
        _idProvider = idProvider;
        _userContextProvider = userContextProvider;
        _logger = logger;
    }
    
    public async Task<CategoryReadModel> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.Categories.AnyAsync(x => x.Name == request.Name, cancellationToken))
            throw new CategoryAlreadyExistsException(request.Name);

        var category = Category.CreateNew(request.Name, _idProvider);
        
        await _dbContext.Categories.AddAsync(category, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("User with id `{@userId}` created new product category (id `{@categoryId}`) with name {@categoryName}.",
            _userContextProvider.UserId, category.Id.Value, category.Name);
        
        var result = CategoryReadModel.FromCategory(category);
        return result;
    }
}

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3);
    }
}