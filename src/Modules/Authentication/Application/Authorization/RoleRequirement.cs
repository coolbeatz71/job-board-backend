using Microsoft.AspNetCore.Authorization;

namespace Authentication.Application.Authorization;

public class RoleRequirement(params string[] allowedRoles) : IAuthorizationRequirement
{
    public string[] AllowedRoles { get; } = allowedRoles ?? throw new ArgumentNullException(nameof(allowedRoles));
}