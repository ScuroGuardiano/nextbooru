namespace UltraHornyBoard.Services;

public interface IAuthenticationService
{
    /// <summary>
    /// Tries to Authenticate the user. Throws on auth failure.
    /// </summary>
    /// <exception cref="UltraHornyBoard.Exceptions.HttpResponseException">When auth failed from wrong username or password</exception>
    /// <param name="userData"></param>
    /// <returns></returns>
    Task<string> AuthenticateUser(Dto.UserLoginRequest userData);
}
