using Nextbooru.Shared;

namespace Nextbooru.Core.Exceptions;

public class UnsupportedMediaTypeException : Exception, IConvertibleToApiErrorResponse
{
    public UnsupportedMediaTypeException(string message)
        : base(message) {}

    public ApiErrorResponse ToApiErrorResponse()
    {
        return new ApiErrorResponse()
        {
            StatusCode = StatusCodes.Status415UnsupportedMediaType,
            Message = Message,
            ErrorCLRType = GetType().FullName,
            ErrorCode = ApiErrorCodes.UnsupportedMediaType
        };
    }
}