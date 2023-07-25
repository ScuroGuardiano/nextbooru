namespace UltraHornyBoard.Dto;

public class LoginUser
{
    [Required]
    public string? Username { get; set; }

    [Required]
    public string? Password { get; set; }
}
