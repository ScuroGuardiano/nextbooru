using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UltraHornyBoard.Core.Services;
using UltraHornyBoard.Core.Dto;

namespace UltraHornyBoard.Core.Controllers;

[Authorize]
[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService userService;

    public UsersController(IUserService userService)
    {
        this.userService = userService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<ExtendedUserInfo>> GetMe()
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
        {
            return StatusCode((int)HttpStatusCode.Unauthorized, new ApiError
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Message = "Authentication token is not valid.",
                ErrorType = "AuthenticationTokenInvalid"
            });
        }

        var user = await userService.GetUserById(Guid.Parse(userId));

        if (user is null)
        {
            return StatusCode((int)HttpStatusCode.NotFound, new ApiError
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Currently authenticated user was not found.",
                ErrorType = "CurrentUserNotFound"
            });
        }

        return new ExtendedUserInfo(user);
    }
}
