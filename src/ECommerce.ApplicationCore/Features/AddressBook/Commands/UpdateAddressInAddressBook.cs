using ECommerce.ApplicationCore.Features.AddressBook.Exceptions;
using ECommerce.ApplicationCore.Shared.Abstractions;
using ECommerce.ApplicationCore.Shared.CQRS;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.ApplicationCore.Features.AddressBook.Commands;

public record UpdateAddressInAddressBookCommand(long Id, string Street, string PostalCode, string City) : ICommand;

internal sealed class UpdateAddressInAddressBookCommandHandler : ICommandHandler<UpdateAddressInAddressBookCommand>
{
    private readonly IAppDbContext _dbContext;
    private readonly IUserContextProvider _userContextProvider;

    public UpdateAddressInAddressBookCommandHandler(IAppDbContext dbContext, IUserContextProvider userContextProvider)
    {
        _dbContext = dbContext;
        _userContextProvider = userContextProvider;
    }
    
    public async ValueTask HandleAsync(UpdateAddressInAddressBookCommand inAddressBookCommand, CancellationToken cancellationToken)
    {
        var customerId = _userContextProvider.UserId!.Value;

        var address = await _dbContext.Addresses.FirstOrDefaultAsync(x => x.Id == inAddressBookCommand.Id && x.CustomerId == customerId, cancellationToken);
        if (address is null)
            throw new AddressNotFoundException(inAddressBookCommand.Id);

        address.Street = inAddressBookCommand.Street;
        address.PostalCode = inAddressBookCommand.PostalCode;
        address.City = inAddressBookCommand.City;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

internal sealed class UpdateAddressCommandValidator : AbstractValidator<UpdateAddressInAddressBookCommand>
{
    public UpdateAddressCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.PostalCode).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
    }
}