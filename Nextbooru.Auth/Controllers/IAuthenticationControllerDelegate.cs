using Microsoft.AspNetCore.Mvc;
using Nextbooru.Auth.Dto;

namespace Nextbooru.Auth.Controllers;

public interface IAuthenticationControllerDelegate
{
    Task<IActionResult> Register(RegisterUserRequest body, ControllerBase controller);
    Task<IActionResult> Login(LoginUserRequest body, ControllerBase controller);

    ActionResult<SessionResponse> CurrentSession(ControllerBase controller);

    Task<IActionResult> Logout(ControllerBase controller);

    Task<IActionResult> InvalidateSession(ControllerBase controller);
}