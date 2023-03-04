namespace ECommerce.ApplicationCore.Shared.Abstractions;

public interface IUserContextProvider
{
    Guid? UserId { get; }
}