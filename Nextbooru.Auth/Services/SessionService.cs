using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Nextbooru.Auth.Models;
using Nextbooru.Shared;

namespace Nextbooru.Auth.Services;

public class SessionService<TDbContext> : ISessionService
    where TDbContext : DbContext, IAuthDbContext
{
    private readonly TDbContext dbContext;
    private readonly IHttpContextAccessor httpContextAccessor;

    public SessionService(TDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        this.dbContext = dbContext;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<Session?> GetSessionAsync(string sessionId)
    {
        if (!Guid.TryParse(sessionId, out var sessionGuid))
        {
            return null;
        }

        return await dbContext.Sessions
            .Include(s => s.User)
            .Where(s => s.Id == sessionGuid)
            .FirstOrDefaultAsync();
    }

    public async Task<Session?> AccessSessionAsync(string sessionId)
    {
        var context = httpContextAccessor.HttpContext!;
        var session = await GetSessionAsync(sessionId);

        if (session is null)
        {
            return null;
        }

        session.LastAccess = DateTime.UtcNow;
        session.LastIP = HttpHelpers.GetRealRemoteIpAddress(context);

        await dbContext.SaveChangesAsync();
        return session;
    }

    public async Task<Session> CreateSessionAsync(User user)
    {
        var context = httpContextAccessor.HttpContext!;
        var ip = HttpHelpers.GetRealRemoteIpAddress(context);

        var session = new Session
        {
            User = user,
            UserAgent = context.Request.Headers.UserAgent,
            CreatedAt = DateTime.UtcNow,
            LoggedInIP = ip,
            LastIP = ip,
            IsValid = true,
            LastAccess = DateTime.UtcNow
        };

        dbContext.Add(session);
        await dbContext.SaveChangesAsync();
        return session;
    }

    public Session? GetCurrentSessionFromHttpContext()
    {
        var context = httpContextAccessor.HttpContext!;
        var session = (Session?)context.Items[AuthenticationConstants.SessionHttpContextItemKey];
        return session;
    }

    public async Task<Session?> InvalidateCurrentSessionAsync()
    {
        var context = httpContextAccessor.HttpContext!;
        var sessionId = context.User.Claims
            .FirstOrDefault(c => c.Type == AuthenticationConstants.SessionClaimType)?.Value;

        if (sessionId is null)
        {
            return null;
        }

        var session = await GetSessionAsync(sessionId);
        if (session is null)
        {
            return null;
        }
        session.IsValid = false;
        await dbContext.SaveChangesAsync();
        return session;
    }
}
