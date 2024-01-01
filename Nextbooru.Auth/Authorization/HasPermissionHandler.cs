using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Nextbooru.Auth.Services;

namespace Nextbooru.Auth.Authorization;

public class HasPermissionHandler : AuthorizationHandler<HasPermissionRequirement>
{
    private readonly IAuthorizationManager authorizationManager;

    public HasPermissionHandler(IAuthorizationManager authorizationManager)
    {
        this.authorizationManager = authorizationManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
    {
        var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim?.Value, out Guid userId))
        {
            return;
        }

        var hasPermision = await authorizationManager.DoesUserHasPermissionAsync(userId, requirement.PermissionKey);

        if (hasPermision)
        {
            context.Succeed(requirement);
        }
    }
}
