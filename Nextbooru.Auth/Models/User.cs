using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Nextbooru.Shared;

namespace Nextbooru.Auth.Models;

[Index(nameof(Username), IsUnique = true)]
public class User : BaseEntity
{
    public Guid Id { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public required string Username { get; set; }

    [Required]
    public required string DisplayName { get; set; }

    [Required]
    public required string HashedPassword { get; set; }

    public DateTime? BannedUntil { get; set; }

    /// <summary>
    /// If set to true then User has permissions to do virtually anything they want to.
    /// </summary>
    public bool IsAdmin { get; set; } = false;

    public List<UserPermission> Permissions { get; set; } = [];
    public List<Role> Roles { get; set; } = [];
}
