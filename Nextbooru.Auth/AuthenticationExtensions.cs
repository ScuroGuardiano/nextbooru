using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nextbooru.Auth.Controllers;
using Nextbooru.Auth.Models;
using Nextbooru.Auth.Services;

namespace Nextbooru.Auth;

public static class AuthenticationExtensions
{
    public static void AddSGAuthentication<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext, IAuthDbContext
    {
        AddSGAuthentication<SessionService<TDbContext>, UserService<TDbContext>, User, Session>(services);
    }

    public static void AddSGAuthentication<TDbContext, TUser, TSession>(this IServiceCollection services)
        where TDbContext : DbContext, IAuthDbContext<TUser, TSession>
        where TSession: Session, new()
        where TUser: User, new()
    {
        AddSGAuthentication<
            SessionService<TDbContext, TSession, TUser>,
            UserService<TDbContext, TUser, TSession>,
            TUser,
            TSession
        >(services);
    }

    public static void AddSGAuthentication<TSessionService, TUserService, TUser, TSession>(this IServiceCollection services, bool addControllers = true)
        where TSessionService : class, ISessionService<TSession>
        where TUserService : class, IUserService<TUser>
        where TSession: Session
        where TUser: User
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ISessionService<TSession>, TSessionService>();
        services.AddScoped<IUserService<TUser>, TUserService>();
        services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());

        if (addControllers)
        {
            services.AddScoped<IAuthenticationControllerDelegate, AuthenticationControllerDelegate<TUser, TSession>>();
            services.AddMvc().AddApplicationPart(typeof(AuthenticationController).Assembly);
        }

        services.AddAuthentication(AuthenticationConstants.AuthenticationScheme)
        .AddCookie(AuthenticationConstants.AuthenticationScheme, o =>
        {
            o.Cookie.HttpOnly = true;

            o.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };

            o.Events.OnValidatePrincipal = async context =>
            {
                if (context.Principal is null)
                {
                    context.RejectPrincipal();
                    return;
                }

                var httpCtx = context.HttpContext;
                var sessionService = httpCtx.RequestServices.GetRequiredService<ISessionService<TSession>>();

                var sessionId = context.Principal.Claims
                    .FirstOrDefault(c => c.Type == AuthenticationConstants.SessionClaimType)?.Value;

                if (sessionId is null)
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync();
                    return;
                }

                var session = await sessionService.AccessSessionAsync(sessionId);
                if (!await sessionService.IsSessionValidAsync(session))
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync();
                    return;
                }

                // Okey, I load session to httpCtx here because I want to make only 1 call to database in request
                // And I can't use loading session middleware before UseAuthentication, because session depends on prinpical, so... yeah
                httpCtx.Items[AuthenticationConstants.SessionHttpContextItemKey] = session;
            };
        });
    }
}
