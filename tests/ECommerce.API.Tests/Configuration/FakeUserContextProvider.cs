using ECommerce.ApplicationCore.Shared.Abstractions;

namespace ECommerce.API.Tests.Configuration;

public class FakeUserContextProvider : IUserContextProvider
{
    public static Guid? CurrentUserId { get; set; }
    public Guid? UserId => CurrentUserId;
}