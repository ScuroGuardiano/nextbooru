using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using UltraHornyBoard.Core.Services;
using UltraHornyBoard.Core.Dto;

namespace UltraHornyBoard.Core.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly AppSettings configuration;
    private readonly IUserService userService;
    private readonly IAuthenticationService authenticationService;

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
            return StatusCode((int)HttpStatusCode.Forbidden, new ApiError
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
                ErrorType = "RegistrationDisabled",
                Message = "Registration is disabled on this server."
            });
        }

        var user = await userService.CreateUser(userDto);

        HttpContext.Response.StatusCode = (int)HttpStatusCode.Created;
        return new ExtendedUserInfo(user);
    }


    [HttpPost("login")]
    public async Task<ActionResult<UserLoginResponse>> Login(UserLoginRequest userDto)
    {
        if (configuration.DisableLogin)
        {
            return StatusCode((int)HttpStatusCode.Forbidden, new ApiError
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
                ErrorType = "LoginDisabled",
                Message = "Login is disabled on this server."
            });
        }

        var token = await authenticationService.AuthenticateUser(userDto);

        return new UserLoginResponse { AccessToken = token };
    }
}
