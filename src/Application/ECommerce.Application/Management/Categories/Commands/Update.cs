using ECommerce.Application.Management.Categories.Exceptions;
using ECommerce.Application.Shared.Abstractions;
using ECommerce.Application.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Management.Categories.Commands;

public class UpdateCategoryCommand : ICommand 
{
    public required long Id { get; init; }
    public required string Name { get; init; }
}

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;

    public UpdateCategoryCommandHandler(IAppDbContext dbContext,
        IUserContextProvider userContextProvider,
        ILogger<UpdateCategoryCommandHandler> logger)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
        _logger = logger;
    }
    
    public async ValueTask HandleAsync(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        // if category with this name already exists, return AlreadyExistsError
        if (await _dbContext.Categories.AnyAsync(x => x.Name == request.Name, cancellationToken))
            throw new CategoryAlreadyExistsException(request.Name);
        
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            throw new CategoryNotFoundException(request.Id);

        var oldCategoryName = category.Name;
        category.Name = request.Name;
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("User with id `{@userId}` updated product category (id `{@categoryId}`) name from {@oldCategoryName} to {@newCategoryName}.",
            _userContextProvider.UserId, category.Id.Value, request.Name, oldCategoryName);
    }
}

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3);
    }
}