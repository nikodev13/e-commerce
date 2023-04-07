using System.Security.Claims;
using ECommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ECommerce.Infrastructure.Authorization;

public class RegisteredCustomerRequirement : IAuthorizationRequirement { } 

public class RegisteredCustomerRequirementHandler : AuthorizationHandler<RegisteredCustomerRequirement>
{
    private readonly AppDbContext _dbContext;

    public RegisteredCustomerRequirementHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RegisteredCustomerRequirement requirement)
    {
        var idClaim = context.User.FindFirst(JwtRegisteredClaimNames.Sub);
        if (idClaim is null || !Guid.TryParse(idClaim.Value, out var id)) return;
        
        if (!await _dbContext.Customers.AnyAsync(x => x.Id == id))
            context.Fail(new AuthorizationFailureReason(this, "This action is only allowed to registered customers."));
    }
}