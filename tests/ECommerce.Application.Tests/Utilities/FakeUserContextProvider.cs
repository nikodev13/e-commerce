using ECommerce.ApplicationCore.Shared.Abstractions;

namespace ECommerce.Application.Tests.Utilities;

public class FakeUserContextProvider : IUserContextProvider
{
    public Guid? UserId => CurrentUserId;

    public static Guid? CurrentUserId { get; set; }
}