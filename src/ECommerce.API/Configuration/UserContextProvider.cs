using System.Security.Claims;
using ECommerce.ApplicationCore.Shared.Abstractions;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ECommerce.API.Configuration;

public class UserContextProvider : IUserContextProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserContextProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Guid? UserId 
        => Guid.TryParse(_contextAccessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out var guid) ? guid : null;
}