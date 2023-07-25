namespace UltraHornyBoard.Dto;

public class RegisterUser
{
    [
        Required,
        MaxLength(16),
        MinLength(3),
        RegularExpression(@"^[a-zA-Z0-9]+[a-zA-Z0-9_]*[a-zA-Z0-9]$")
    ]
    public string? Username { get; set; }

    [
        Required,
        EmailAddress
    ]
    public string? Email { get; set; }

    [
        Required,
        MinLength(8),
        MaxLength(72)
    ]
    public string? Password { get; set; }
}
