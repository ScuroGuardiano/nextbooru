namespace Nextbooru.Core.Dto;

public class ChangePasswordRequest
{
    [Required]
    public required string CurrentPassword { get; init; }

    [
        Required,
        MinLength(8),
        MaxLength(72)
    ]
    public required string NewPassword { get; init; }
}
