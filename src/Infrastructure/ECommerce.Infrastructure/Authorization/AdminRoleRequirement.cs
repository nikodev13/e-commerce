using System.IdentityModel.Tokens.Jwt;
using ECommerce.Application.Common.Interfaces;
using ECommerce.Application.Users.Models;
using ECommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;

namespace ECommerce.Infrastructure.Authorization;

public class AdminRoleRequirement : IAuthorizationRequirement
{
}

public class AdminRoleRequirementHandler : AuthorizationHandler<AdminRoleRequirement>
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IUserContextService _contextService;

    public AdminRoleRequirementHandler(ApplicationDbContext applicationDbContext, IUserContextService contextService)
    {
        _applicationDbContext = applicationDbContext;
        _contextService = contextService;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminRoleRequirement requirement)
    {
        var idClaim = context.User.FindFirst(JwtRegisteredClaimNames.Sub);
        if (idClaim is null || !Guid.TryParse(idClaim.Value, out var id))
        {
            return Task.CompletedTask;
        }
        
        if (_applicationDbContext.Users.Any(x => x.Id == id && x.Role == Role.Admin))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}