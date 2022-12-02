using System.Security.Claims;
using ECommerce.Application.Common.Interfaces;

namespace ECommerce.API.Configuration;

public class UserContextService : IUserContextService
{
    private readonly HttpContext _context;

    public UserContextService(HttpContext context)
    {
        _context = context;
    }

    public ClaimsPrincipal User => _context.User;
    public Guid? UserId => Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var guid) ? guid : null;
}