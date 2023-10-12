using Nextbooru.Shared;

namespace Nextbooru.Core.Exceptions;

public class InvalidImageFileException : Exception, IConvertibleToApiErrorResponse
{
    public InvalidImageFileException()
        : base("Provided file is not valid image file.")
    {
    }
    
    public ApiErrorResponse ToApiErrorResponse()
    {
        return new ApiErrorResponse()
        {
            StatusCode = StatusCodes.Status400BadRequest,
            ErrorCode = ApiErrorCodes.InvalidImageFile,
            ErrorCLRType = GetType().FullName,
            Message = Message
        };
    }
}
