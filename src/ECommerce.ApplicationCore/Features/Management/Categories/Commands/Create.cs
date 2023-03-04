using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Management.Categories.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.ApplicationCore.Features.Management.Categories.Commands;

public class CreateCategoryCommand : ICommand<long>
{
    public required string Name { get; init; }
}

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, long>
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
    
    public async ValueTask<long> HandleAsync(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        if (await _dbContext.Categories.AnyAsync(x => x.Name == request.Name, cancellationToken))
            throw new CategoryAlreadyExistsException(request.Name);

        var category = new Category
        {
            Id = _idProvider.GenerateId(),
            Name = request.Name
        };

        await _dbContext.Categories.AddAsync(category, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("User with id `{@userId}` created new product category (id `{@categoryId}`) with name {@categoryName}.",
            _userContextProvider.UserId, category.Id, category.Name);

        return category.Id;
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