using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nextbooru.Auth.Dto;
using Nextbooru.Auth.Services;
using Nextbooru.Shared;

namespace Nextbooru.Auth.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserService userService;
    private readonly ISessionService sessionService;

    public AuthenticationController(IUserService userService, ISessionService sessionService)
    {
        this.userService = userService;
        this.sessionService = sessionService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest body)
    {
        try
        {
            var user = await userService.RegisterUserAsync(body);
            var principal = userService.UserToClaimsPrincipal(user, AuthenticationConstants.AuthenticationScheme);
            var session = await sessionService.CreateSessionAsync(user);
            principal.Identities.First().AddClaim(new(AuthenticationConstants.SessionClaimType, session.Id.ToString()));
            await HttpContext.SignInAsync(AuthenticationConstants.AuthenticationScheme, principal);

            return new JsonResult(new SessionResponse(session))
            {
                StatusCode = StatusCodes.Status201Created
            };
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

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest body)
    {
        try
        {
            var user = await userService.AuthenticateUser(body);
            var principal = userService.UserToClaimsPrincipal(user, AuthenticationConstants.AuthenticationScheme);
            var session = await sessionService.CreateSessionAsync(user);
            principal.Identities.First().AddClaim(new (AuthenticationConstants.SessionClaimType, session.Id.ToString()));
            await HttpContext.SignInAsync(AuthenticationConstants.AuthenticationScheme, principal);
            return new JsonResult(new SessionResponse(session));
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

    [HttpGet("currentSession")]
    [Authorize]
    public ActionResult<SessionResponse> CurrentSession()
    {
        var session = sessionService.GetCurrentSessionFromHttpContext();
        // Session should always be true in Authorized route
        // So throw the shit away if it's null
        return new SessionResponse(session!);
    }

    [Authorize]
    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        await sessionService.InvalidateCurrentSessionAsync();
        await HttpContext.SignOutAsync(AuthenticationConstants.AuthenticationScheme);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("invalidate-session")]
    public async Task<IActionResult> InvalidateSession()
    {
        await sessionService.InvalidateCurrentSessionAsync();
        return NoContent();
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
