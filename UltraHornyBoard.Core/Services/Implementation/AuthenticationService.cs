using System.Net;
using BC = BCrypt.Net.BCrypt;
using UltraHornyBoard.Core.Dto;
using UltraHornyBoard.Core.Exceptions;
using UltraHornyBoard.Core.Models;

namespace UltraHornyBoard.Core.Services.Implementation;

public class AuthenticationService : IAuthenticationService
{
    private readonly HornyContext context;
    private readonly IJwtService jwtService;

    public AuthenticationService(HornyContext context, IJwtService jwtService)
    {
        this.context = context;
        this.jwtService = jwtService;
    }

    public async Task<string> AuthenticateUser(UserLoginRequest userData)
    {
        var username = userData.Username.ToLower();
        var user = await context.Users
            .Where(user => user.Username == username)
            .FirstAsync();

        if (user is null)
        {
            throw new HttpResponseException(
                (int)HttpStatusCode.Unauthorized,
                "InvalidUsernameOrPassword",
                "Invalid username or password."
            );
        }

        if (BC.Verify(userData.Password, user.HashedPassword))
        {
            return jwtService.SignToken(user.Id.ToString());    
        }

        throw new HttpResponseException(
            (int)HttpStatusCode.Unauthorized,
            "InvalidUsernameOrPassword",
            "Invalid username or password."
        );
    }
}
