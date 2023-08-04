namespace UltraHornyBoard.Models;

public class User : BaseEntity
{
    public Guid Id { get; set; }

    [Required]
    public required string Email { get; set; }

    [Required]
    public required string Username { get; set; }

    [Required]
    public required string DisplayName { get; set; }

    [Required]
    public required string HashedPassword { get; set; }

    public DateTime? BannedUntil { get; set; } = null;

    // Simple way for now
    public bool IsAdmin { get; set; } = false;
}
