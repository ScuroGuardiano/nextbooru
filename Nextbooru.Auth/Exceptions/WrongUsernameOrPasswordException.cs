using System.Net;
using Nextbooru.Shared;

namespace Nextbooru.Auth.Exceptions;

public class WrongUsernameOrPasswordException : Exception, IConvertibleToApiErrorResponse
{
    public WrongUsernameOrPasswordException()
        : base($"Wrong username or password.")
    {
    }

    public ApiErrorResponse ToApiErrorResponse()
    {
        return new() {
            StatusCode = (int)HttpStatusCode.Unauthorized,
            ErrorCode = ApiErrorCodes.WrongUsernameOrPassword,
            ErrorCLRType = GetType().FullName,
            Message = Message
        };
    }
}
