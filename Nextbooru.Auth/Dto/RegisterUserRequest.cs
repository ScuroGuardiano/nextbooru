using System.ComponentModel.DataAnnotations;

namespace Nextbooru.Auth.Dto;

public class RegisterUserRequest
{
    [Required]
    [MaxLength(16)]
    [MinLength(3)]
    public required string Username { get; set; }

    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [MaxLength(72)]
    [MinLength(8)]
    public required string Password { get; set; }
}
