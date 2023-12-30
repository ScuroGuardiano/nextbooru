using Microsoft.AspNetCore.Http;

namespace Nextbooru.Shared.Exceptions;

public class RecordAlreadyExistsException : Exception, IConvertibleToApiErrorResponse
{
    public RecordAlreadyExistsException(object? resourceId = null, string? resourceType = null)
        : base(FormatErrorMessage(resourceId, resourceType))
    { }

    public ApiErrorResponse ToApiErrorResponse()
    {
        return new ApiErrorResponse()
        {
            StatusCode = StatusCodes.Status409Conflict,
            ErrorCode = ApiErrorCodes.RecordAlreadyExists,
            ErrorCLRType = GetType().FullName,
            Message = Message
        };
    }

    private static string FormatErrorMessage(object? resourceId, string? resourceType)
    {
        var subject = resourceType ?? "Record";

        return resourceId is not null
            ? $"{subject} `{resourceId}` already exists."
            : $"{subject} already exists.";
    }
}
