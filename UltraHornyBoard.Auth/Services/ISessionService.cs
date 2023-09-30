using UltraHornyBoard.Auth.Models;

namespace UltraHornyBoard.Auth.Services;

public interface ISessionService<TSession>
where TSession : Session
{
    Task<TSession?> GetSessionAsync(string sessionId);

    Task<TSession?> AccessSessionAsync(string sessionId);

    TSession? GetCurrentSessionFromHttpContext();

    Task<TSession> CreateSessionAsync(User user);

    Task<TSession?> InvalidateCurrentSessionAsync();

    Task<bool> IsSessionValidAsync(TSession? session)
    {
        return Task.FromResult(session is not null && session.IsValid);
    }
}
