using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Authentication.Application.Authorization.Handlers;

public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        RoleRequirement requirement)
    {
        var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userRole))
        {
            return Task.CompletedTask;
        }

        if (requirement.AllowedRoles.Contains(userRole, StringComparer.OrdinalIgnoreCase))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}