namespace Nextbooru.Auth.Models;

public class Session
{
    public Guid Id { get; set; }

    // FIXME: This should be generic or thrown away
    public User? User { get; set; }

    public Guid UserId { get; set; }

    public bool IsValid { get; set; }

    public string? LoggedInIP { get; set; }

    public string? LastIP { get; set; }

    public string? UserAgent { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastAccess { get; set; }
}
