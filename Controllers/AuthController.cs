using Microsoft.AspNetCore.Mvc;
using UltraHornyBoard.Services;
using UltraHornyBoard.Dto;
using Microsoft.Extensions.Options;
using System.Net;

namespace UltraHornyBoard.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private AppSettings configuration;
    private IUserService userService;
    private IAuthenticationService authenticationService;

    public AuthController(
        IOptions<AppSettings> configuration,
        IUserService userService,
        IAuthenticationService authenticationService
    )
    {
        this.configuration = configuration.Value;
        this.userService = userService;
        this.authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ExtendedUserInfo>> Register(UserRegisterRequest userDto)
    {
        if (configuration.DisableRegistration)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, new ApiError {
                StatusCode = (int)HttpStatusCode.Forbidden,
                ErrorType = "RegistrationDisabled",
                Message = "Registration is disabled on this server."
            });
        }

        var user = await userService.CreateUser(userDto);

        return Created($"/users/{user.Username}", new ExtendedUserInfo(user));
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResponse>> Login(UserLoginRequest userDto)
    {
        if (configuration.DisableLogin)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, new ApiError {
                StatusCode = (int)HttpStatusCode.Forbidden,
                ErrorType = "LoginDisabled",
                Message = "Login is disabled on this server."
            });
        }

        var token = await authenticationService.AuthenticateUser(userDto);

        return new UserLoginResponse { AccessToken = token };
    }
}
