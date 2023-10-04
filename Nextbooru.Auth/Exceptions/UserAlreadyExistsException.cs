using System.Net;
using Nextbooru.Shared;

namespace Nextbooru.Auth.Exceptions;

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
            ErrorCode = ApiErrorCodes.UserAlreadyExists,
            ErrorCLRType = GetType().FullName,
            Message = Message
        };
    }
}
