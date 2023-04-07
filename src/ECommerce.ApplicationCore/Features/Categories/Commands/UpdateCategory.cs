using ECommerce.ApplicationCore.Features.Categories.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ECommerce.ApplicationCore.Features.Categories.Commands;

public record UpdateCategoryCommand(long Id, string Name) : ICommand;

internal sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
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

        var userId = _userContextProvider.UserId!.Value;

        var oldCategoryName = category.Name;
        category.Name = request.Name;
        category.LastModifiedBy = userId;
        category.LastModifiedAt = DateTime.Now;
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("User with id `{@userId}` updated product category (id `{@categoryId}`) name from {@oldCategoryName} to {@newCategoryName}.",
            userId, category.Id, request.Name, oldCategoryName);
    }
}

internal sealed class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
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