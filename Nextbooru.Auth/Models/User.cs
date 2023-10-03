using System.ComponentModel.DataAnnotations;
using Nextbooru.Shared;

namespace Nextbooru.Auth.Models;

public class User : BaseEntity
{
    public Guid Id { get; set; }

    [Required]
    public string? Email { get; set; }

    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string DisplayName { get; set; } = null!;

    [Required]
    public string HashedPassword { get; set; } = null!;

    public DateTime? BannedUntil { get; set; } = null;

    // Simple way for now
    public bool IsAdmin { get; set; } = false;
}
