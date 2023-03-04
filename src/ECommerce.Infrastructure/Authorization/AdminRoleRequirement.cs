using System.IdentityModel.Tokens.Jwt;
using ECommerce.ApplicationCore.Entities;
using ECommerce.ApplicationCore.Shared.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Authorization;

public class AdminRoleRequirement : IAuthorizationRequirement { }

public class AdminRoleRequirementHandler : AuthorizationHandler<AdminRoleRequirement>
{
    private readonly IAppDbContext _dbContext;

    public AdminRoleRequirementHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRoleRequirement requirement)
    {
        var idClaim = context.User.FindFirst(JwtRegisteredClaimNames.Sub);
        if (idClaim is null || !Guid.TryParse(idClaim.Value, out var id)) return;
        
        if (await _dbContext.Users.AnyAsync(x => x.Id == id && x.Role == Role.Admin))
        {
            context.Succeed(requirement);
        }
    }
}