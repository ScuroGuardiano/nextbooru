using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Nextbooru.Auth.Dto;
using Nextbooru.Auth.Models;
using Nextbooru.Auth.Services;
using Nextbooru.Shared;

namespace Nextbooru.Auth.Controllers;

public class AuthenticationControllerDelegate<TUser, TSession> : IAuthenticationControllerDelegate
    where TUser : User
    where TSession : Session
{

    readonly IUserService<TUser> userService;
    readonly ISessionService<TSession> sessionService;

    public AuthenticationControllerDelegate(IUserService<TUser> userService, ISessionService<TSession> sessionService)
    {
        this.userService = userService;
        this.sessionService = sessionService;
    }

    public async Task<IActionResult> InvalidateSession(ControllerBase controller)
    {
        await sessionService.InvalidateCurrentSessionAsync();
        return controller.NoContent();
    }

    public async Task<IActionResult> Login(LoginUserRequest body, ControllerBase controller)
    {
        var httpContext = controller.HttpContext;

        try
        {
            var user = await userService.AuthenticateUser(body);
            var principal = userService.UserToClaimsPrincipal(user, AuthenticationConstants.AuthenticationScheme);
            var session = await sessionService.CreateSessionAsync(user);
            principal.Identities.First().AddClaim(new (AuthenticationConstants.SessionClaimType, session.Id.ToString()));
            await httpContext.SignInAsync(AuthenticationConstants.AuthenticationScheme, principal);
            return controller.NoContent();
        }
        catch(Exception exception)
        {
            if (exception is IConvertibleToApiErrorResponse apiException)
            {
                return ResultFromException(apiException);
            }

            throw;
        }
    }

    public async Task<IActionResult> Logout(ControllerBase controller)
    {
        var httpContext = controller.HttpContext;

        await sessionService.InvalidateCurrentSessionAsync();
        await httpContext.SignOutAsync(AuthenticationConstants.AuthenticationScheme);
        return controller.NoContent();
    }

    public ActionResult<SessionResponse> CurrentSession(ControllerBase controller)
    {
        var session = sessionService.GetCurrentSessionFromHttpContext();
        // Session should always be true in Authorized route
        // So throw the shit away if it's null
        return new SessionResponse(session!);
    }

    public async Task<IActionResult> Register(RegisterUserRequest body, ControllerBase controller)
    {
        var httpContext = controller.HttpContext;

        try
        {
            var user = await userService.RegisterUserAsync(body);
            var principal = userService.UserToClaimsPrincipal(user, AuthenticationConstants.AuthenticationScheme);
            var session = await sessionService.CreateSessionAsync(user);
            principal.Identities.First().AddClaim(new(AuthenticationConstants.SessionClaimType, session.Id.ToString()));
            await httpContext.SignInAsync(AuthenticationConstants.AuthenticationScheme, principal);

            return new StatusCodeResult((int)HttpStatusCode.Created);
        }
        catch (Exception exception)
        {
            if (exception is IConvertibleToApiErrorResponse apiException)
            {
                return ResultFromException(apiException);
            }

            throw;
        }
    }

    private IActionResult ResultFromException(IConvertibleToApiErrorResponse ex)
    {
        var apiErr = ex.ToApiErrorResponse();
        return new JsonResult(apiErr)
        {
            StatusCode = apiErr.StatusCode
        };
    }
}
