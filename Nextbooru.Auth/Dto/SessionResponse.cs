using System.Diagnostics.CodeAnalysis;
using Nextbooru.Auth.Models;

namespace Nextbooru.Auth.Dto;

public class SessionResponse
{
    public SessionResponse() { }

    [SetsRequiredMembers]
    public SessionResponse(Session session)
    {
        Id = session.Id;
        User = new UserResponse
        {
            Id = session.User!.Id,  // User should be always set here, so error if it'll be null is good
            Username = session.User.Username
        };
        LoggedInIP = session.LoggedInIP;
        LastIP = session.LastIP;
        UserAgent = session.UserAgent;
        CreatedAt = session.CreatedAt;
        LastAccess = session.LastAccess;
    }


    public Guid Id { get; init; }

    public required UserResponse User { get; init; }

    public string? LoggedInIP { get; set; }

    public string? LastIP { get; set; }

    public string? UserAgent { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastAccess { get; set; }
}
