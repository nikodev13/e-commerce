using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Features.Categories.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.ApplicationCore.Features.Categories.Commands;

public record CreateCategoryCommand(string Name) : ICommand<long>;

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

        var userId = _userContextProvider.UserId!.Value;
        
        var category = new Category
        {
            Id = _idProvider.GenerateId(),
            Name = request.Name,
            CreatedBy = userId,
            CreatedAt = DateTime.Now
        };

        await _dbContext.Categories.AddAsync(category, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("User with id `{@userId}` created new product category (id `{@categoryId}`) with name {@categoryName}.",
            userId, category.Id, category.Name);

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