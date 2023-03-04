using ECommerce.ApplicationCore.Features.Customer.Adresses.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.Customer.Adresses.Commands;

public class UpdateAddressCommand : ICommand
{
    public required long Id { get; init; }
    public required string Street { get; init; }
    public required string PostalCode { get; init; }
    public required string City { get; init; }
}

public class UpdateAddressCommandHandler : ICommandHandler<UpdateAddressCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public UpdateAddressCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask HandleAsync(UpdateAddressCommand command, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;

        var address = await _dbContext.Addresses.FirstOrDefaultAsync(x => x.Id == command.Id && x.CustomerId == customerId, cancellationToken);
        if (address is null)
            throw new AddressNotFoundException(command.Id);

        address.Street = command.Street;
        address.PostalCode = command.PostalCode;
        address.City = command.City;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

public class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressCommand>
{
    public UpdateAddressCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.PostalCode).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
    }
}