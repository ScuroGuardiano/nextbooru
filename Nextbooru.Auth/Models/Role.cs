using System.ComponentModel.DataAnnotations;

namespace Nextbooru.Auth.Models;

public class Role
{
    [Key]
    public required string Name { get; set; }

    public string? Description { get; set; }

    public List<RolePermission> Permissions { get; set; } = [];

    public List<User> Users { get; set; } = [];
}

