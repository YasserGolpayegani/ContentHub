using ContentHub.Api.Authorization.Requirements;
using ContentHub.Application.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace ContentHub.Api.Authorization.Handlers;

public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
            return Task.CompletedTask;

        bool hasPermission = context.User.Claims
            .Where(c => c.Type == CustomClaimTypes.Permission)
            .Any(c => c.Value == requirement.Permission);

        if (hasPermission)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
