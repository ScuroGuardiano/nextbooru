namespace Nextbooru.Auth.Models;

public class RolePermission
{
    public required string PermissionKey { get; set; }
    public Role? Role { get; set; }
    public required string RoleName { get; set; }
}
