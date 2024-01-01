using Microsoft.AspNetCore.Authorization;

namespace Nextbooru.Auth.Authorization;

public class HasPermissionRequirement(string permissionKey) : IAuthorizationRequirement
{
    public string PermissionKey { get; } = permissionKey;
}
