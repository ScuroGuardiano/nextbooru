using System.ComponentModel.DataAnnotations;

namespace Nextbooru.Auth.Models;

public class Role
{
    [Key]
    public required string Name { get; set; }

    public string? Description { get; set; }

    /// <summary>
    /// Builtin roles are protected from deletion.
    ///
    /// TODO: maybe create database trigger to protect that even stronger? :D
    /// </summary>
    public bool Builtin { get; set; }

    public List<RolePermission> Permissions { get; set; } = [];

    public List<User> Users { get; set; } = [];
}

