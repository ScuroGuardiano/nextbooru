namespace UltraHornyBoard.Services;

public interface IAuthenticationService
{
    Task<string> AuthenticateUser(Dto.LoginUser userData);
}
