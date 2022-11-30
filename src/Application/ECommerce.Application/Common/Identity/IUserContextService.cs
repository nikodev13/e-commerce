using System.Security.Claims;

namespace ECommerce.Application.Common.Identity;

public interface IUserContextService
{
    ClaimsPrincipal User { get; }
    Guid? UserId { get; }
}