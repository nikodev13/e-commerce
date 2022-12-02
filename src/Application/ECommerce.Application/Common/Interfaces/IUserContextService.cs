using System.Security.Claims;

namespace ECommerce.Application.Common.Interfaces;

public interface IUserContextService
{
    ClaimsPrincipal User { get; }
    Guid? UserId { get; }
}