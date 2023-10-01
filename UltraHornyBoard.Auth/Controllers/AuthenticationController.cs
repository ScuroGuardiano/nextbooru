using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltraHornyBoard.Auth.Dto;
namespace UltraHornyBoard.Auth.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    readonly IAuthenticationControllerDelegate authenticationControllerDelegate;

    public AuthenticationController(IAuthenticationControllerDelegate authenticationControllerDelegate)
    {
        this.authenticationControllerDelegate = authenticationControllerDelegate;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest body)
    {
        return await authenticationControllerDelegate.Register(body, this);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest body)
    {
        return await authenticationControllerDelegate.Login(body, this);
    }

    [HttpGet("currentSession")]
    [Authorize]
    public ActionResult<SessionResponse> CurrentSession()
    {
        return authenticationControllerDelegate.CurrentSession(this);
    }

    [Authorize]
    [HttpDelete("logout")]
    public async Task<IActionResult> Logout()
    {
        return await authenticationControllerDelegate.Logout(this);
    }

    [Authorize]
    [HttpDelete("invalidate-session")]
    public async Task<IActionResult> InvalidateSession()
    {
        return await authenticationControllerDelegate.InvalidateSession(this);
    }
}
