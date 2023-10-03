namespace Nextbooru.Core.Dto;

public class UserLoginRequest
{
    [Required]
    public required string Username { get; init; }

    [Required]
    public required string Password { get; init; }
}
