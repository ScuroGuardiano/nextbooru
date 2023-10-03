using System.Diagnostics.CodeAnalysis;
using Nextbooru.Auth.Models;

namespace Nextbooru.Auth.Dto;

public class SessionResponse
{
    public SessionResponse() { }

    [SetsRequiredMembers]
    public SessionResponse(Session session)
    {
        User = new UserResponse
        {
            // User should be always set here, so error if it'll be null is good
            Username = session.User!.Username,
            DisplayName = session.User.DisplayName
        };
        LoggedInIP = session.LoggedInIP;
        LastIP = session.LastIP;
        UserAgent = session.UserAgent;
        CreatedAt = session.CreatedAt;
        LastAccess = session.LastAccess;
    }

    // public Guid Id { get; init; }
    // User shouldn't know ID of his session.

    public required UserResponse User { get; init; }

    public string? LoggedInIP { get; set; }

    public string? LastIP { get; set; }

    public string? UserAgent { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastAccess { get; set; }
}
