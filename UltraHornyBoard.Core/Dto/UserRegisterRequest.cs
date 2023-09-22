namespace UltraHornyBoard.Dto;

public class UserRegisterRequest
{
    [
        Required,
        MaxLength(16),
        MinLength(3),
        RegularExpression(@"^[a-zA-Z0-9]+[a-zA-Z0-9_]*[a-zA-Z0-9]$"),
        IsNotOneOfAttribute("me", "admin")
    ]
    public required string Username { get; init; }

    [
        Required,
        EmailAddress
    ]
    public required string Email { get; init; }

    [
        Required,
        MinLength(8),
        MaxLength(72)
    ]
    public required string Password { get; init; }
}
