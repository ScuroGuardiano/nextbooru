using System.Net;
using Nextbooru.Shared;

namespace Nextbooru.Auth.Exceptions;

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
