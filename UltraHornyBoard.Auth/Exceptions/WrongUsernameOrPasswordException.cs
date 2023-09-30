using System.Net;
using UltraHornyBoard.Shared;

namespace UltraHornyBoard.Auth.Exceptions;

public class WrongUsernameOrPasswordException : Exception, IConvertibleToApiErrorResponse
{
    public WrongUsernameOrPasswordException()
        : base($"Wrong username or password.")
    {
    }

    public ApiErrorReponse ToApiErrorResponse()
    {
        return new() {
            StatusCode = (int)HttpStatusCode.Unauthorized,
            ErrorType = GetType().FullName,
            Message = Message
        };
    }
}
