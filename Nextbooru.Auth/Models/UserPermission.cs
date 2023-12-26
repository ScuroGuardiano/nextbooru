namespace Nextbooru.Auth.Models;

public class UserPermission
{
    public required string PermissionKey { get; set; }
    public User? User { get; set; }
    public Guid UserId { get; set; }
}

