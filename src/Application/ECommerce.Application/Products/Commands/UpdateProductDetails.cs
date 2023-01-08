using ECommerce.Application.Common.CQRS;
using ECommerce.Application.Common.Exceptions;
using ECommerce.Application.Common.Interfaces;
using FluentValidation;
using IdGen;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Products.Commands;

public class UpdateProductDetailsCommand : ICommand
{
    public long Id { get; }
    public string Name { get; }
    public string Description { get; }
    public long CategoryId { get; }

    public UpdateProductDetailsCommand(long id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}

public class UpdateProductDetailsCommandValidator : AbstractValidator<UpdateProductDetailsCommand>
{
    public UpdateProductDetailsCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(3);
        RuleFor(x => x.CategoryId)
            .NotEmpty();
    }
}

public class UpdateProductDetailsCommandHandler : ICommandHandler<UpdateProductDetailsCommand>
{
    private readonly IApplicationDatabase _database;

    public UpdateProductDetailsCommandHandler(IApplicationDatabase database)
    {
        _database = database;
    }
    
    public async Task<Unit> Handle(UpdateProductDetailsCommand request, CancellationToken cancellationToken)
    {
        var product = await _database.Products.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (product is null)
            throw new NotFoundException($"Product with ID `{request.Id}` does not exist.");

        var category = await _database.Categories.FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken);
        if (category is null)
            throw new BadRequestException($"Category with id {request.CategoryId} does not exist.");
        
        product.Name = request.Name;
        product.Description = request.Description;
        product.Category = category;

        await _database.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}