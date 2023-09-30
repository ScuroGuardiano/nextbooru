using Microsoft.EntityFrameworkCore;
using UltraHornyBoard.Auth.Models;
using UltraHornyBoard.Shared;

namespace UltraHornyBoard.Auth.Services;

public class SessionService<TDbContext> : SessionService<TDbContext, Session, User>
    where TDbContext : DbContext, IAuthDbContext
{
        public SessionService(TDbContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
        }
}

public class SessionService<TDbContext, TSession> : SessionService<TDbContext, TSession, User>
    where TDbContext : DbContext, IAuthDbContext<User, TSession>
    where TSession: Session, new()
{
        public SessionService(TDbContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
        }
}

public class SessionService<TDbContext, TSession, TUser> : ISessionService<TSession>
    where TDbContext : DbContext, IAuthDbContext<TUser, TSession>
    where TSession : Session, new()
    where TUser: User
{
    private readonly TDbContext dbContext;
    private readonly IHttpContextAccessor httpContextAccessor;

    public SessionService(TDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        this.dbContext = dbContext;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<TSession?> GetSessionAsync(string sessionId)
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

    public async Task<TSession?> AccessSessionAsync(string sessionId)
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

    public async Task<TSession> CreateSessionAsync(User user)
    {
        var context = httpContextAccessor.HttpContext!;
        var ip = HttpHelpers.GetRealRemoteIpAddress(context);

        var session = new TSession {
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

    public TSession? GetCurrentSessionFromHttpContext()
    {
        var context = httpContextAccessor.HttpContext!;
        var session = (TSession?)context.Items[AuthenticationConstants.SessionHttpContextItemKey];
        return session;
    }

    public async Task<TSession?> InvalidateCurrentSessionAsync()
    {
        var context = httpContextAccessor.HttpContext!;
        var sessionId = context.User.Claims
            .Where(c => c.Type == AuthenticationConstants.SessionClaimType)
            .FirstOrDefault()?.Value;

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
