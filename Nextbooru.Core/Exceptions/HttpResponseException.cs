using Nextbooru.Shared;

namespace Nextbooru.Core.Exceptions;

public class HttpResponseException : Exception, IConvertibleToApiErrorResponse
{
    public HttpResponseException(int statusCode, string? errorCode = null, string? message = null) : base(message) =>
        (StatusCode, ErrorCode) = (statusCode, errorCode);

    public int StatusCode { get; }

    public string? ErrorCode { get; }

    public ApiErrorReponse ToApiErrorResponse()
    {
        return new ApiErrorReponse {
            StatusCode = StatusCode,
            ErrorCode = ErrorCode,
            ErrorCLRType = GetType().FullName,
            Message = Message
        };
    }
}
