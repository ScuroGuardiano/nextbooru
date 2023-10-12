using Nextbooru.Shared;

namespace Nextbooru.Core.Exceptions;

public class NotAllowedFileTypeException : Exception, IConvertibleToApiErrorResponse
{
    public NotAllowedFileTypeException(string extension, string contentType)
        : base($"File type {extension} (Content-Type: {contentType}) is not allowed.")
    {
    }
    
    public ApiErrorResponse ToApiErrorResponse()
    {
        return new ApiErrorResponse()
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Message = Message,
            ErrorCLRType = GetType().FullName,
            ErrorCode = ApiErrorCodes.NotAllowedFileType
        };
    }
}
