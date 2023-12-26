namespace Nextbooru.Auth.Models;

public class UserRole
{
    public User? User { get; set; }
    public Guid UserId { get; set; }
    public Role? Role { get; set; }
    public required string RoleName { get; set; }
}
