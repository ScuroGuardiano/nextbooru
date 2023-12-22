using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nextbooru.Auth.Models;
using Nextbooru.Auth.Services;
using Nextbooru.Core.Dto;
using Nextbooru.Core.Dto.Responses;
using Nextbooru.Shared;

namespace Nextbooru.Core.Controllers;

[Authorize]
[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserService<User> userService;

    public UsersController(IUserService<User> userService)
    {
        this.userService = userService;
    }

    [HttpGet("me")]
    public async Task<ActionResult<ExtendedUserInfo>> GetMe()
    {
        string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await userService.GetById(Guid.Parse(userId!));

        if (user is null)
        {
            return StatusCode((int)HttpStatusCode.NotFound, new ApiErrorResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = "Currently authenticated user was not found.",
                ErrorCode = ApiErrorCodes.CurrentUserNotFound
            });
        }

        return new ExtendedUserInfo(user);
    }
}
