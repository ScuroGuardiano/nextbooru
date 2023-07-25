namespace UltraHornyBoard.Services;

public interface IAuthenticationService
{
    Task<Models.User> AuthenticateUser(Dto.LoginUser userData);
}