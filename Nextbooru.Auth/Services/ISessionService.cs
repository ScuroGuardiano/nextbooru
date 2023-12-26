using Nextbooru.Auth.Models;

namespace Nextbooru.Auth.Services;

public interface ISessionService
{
    Task<Session?> GetSessionAsync(string sessionId);

    Task<Session?> AccessSessionAsync(string sessionId);

    Session? GetCurrentSessionFromHttpContext();

    Task<Session> CreateSessionAsync(User user);

    Task<Session?> InvalidateCurrentSessionAsync();

    Task<bool> IsSessionValidAsync(Session? session)
    {
        return Task.FromResult(session is not null && session.IsValid);
    }
}
