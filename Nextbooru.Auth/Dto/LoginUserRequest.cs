using System.ComponentModel.DataAnnotations;

namespace Nextbooru.Auth.Dto;

public class LoginUserRequest
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
}
