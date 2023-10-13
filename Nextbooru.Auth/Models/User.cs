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

    // TODO: There is problem with those required properties
    // I can't mark them as requred, coz it would violate new() type constraint required by
    // Generic services in Auth Project. I should do factory or something I guess.
    
    /// <summary>
    /// Username should be always stored in lowercase.
    /// </summary>
    [Required]
    public string Username { get; set; }

    [Required]
    public string DisplayName { get; set; }

    [Required]
    public string HashedPassword { get; set; }

    public DateTime? BannedUntil { get; set; } = null;

    // Simple way for now
    public bool IsAdmin { get; set; } = false;
}
