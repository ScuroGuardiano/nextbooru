using Nextbooru.Shared;

namespace Nextbooru.Core.Exceptions;

public class FileDoesNotExistsException : Exception, IConvertibleToApiErrorResponse
{
    public FileDoesNotExistsException(string filename)
        : base($"File ${filename} does not exists")
    {
    }
    
    public ApiErrorResponse ToApiErrorResponse()
    {
        return new ApiErrorResponse()
        {
            StatusCode = StatusCodes.Status404NotFound,
            ErrorCode = ApiErrorCodes.FileDoesNotExists,
            ErrorCLRType = this.GetType().FullName,
            Message = Message
        };
    }
}
