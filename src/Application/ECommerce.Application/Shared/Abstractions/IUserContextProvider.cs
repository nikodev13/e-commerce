namespace ECommerce.Application.Shared.Abstractions;

public interface IUserContextProvider
{
    Guid? UserId { get; }
}