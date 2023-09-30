using System.ComponentModel.DataAnnotations;
using UltraHornyBoard.Shared;

namespace UltraHornyBoard.Auth.Models;

public class User : BaseEntity
{
    public Guid Id { get; set; }

    [Required]
    public string? Email { get; set; }

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
