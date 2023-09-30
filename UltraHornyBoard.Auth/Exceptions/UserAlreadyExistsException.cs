using System.Net;
using UltraHornyBoard.Shared;

namespace UltraHornyBoard.Auth.Exceptions;

public class UserAlreadyExistsException : Exception, IConvertibleToApiErrorResponse
{
    public UserAlreadyExistsException(string name)
        : base($"User {name} already exists.")
    {
    }

    public ApiErrorReponse ToApiErrorResponse()
    {
        return new() {
            StatusCode = (int)HttpStatusCode.Conflict,
            ErrorType = GetType().FullName,
            Message = Message
        };
    }
}
