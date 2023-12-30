using Microsoft.AspNetCore.Http;

namespace Nextbooru.Shared.Exceptions;

public class NotFoundException : Exception, IConvertibleToApiErrorResponse
{
    public NotFoundException(object? resourceId = null, string? resourceType = null)
        : base(FormatErrorMessage(resourceId, resourceType))
    {}

    public ApiErrorResponse ToApiErrorResponse()
    {
        return new ApiErrorResponse()
        {
            StatusCode = StatusCodes.Status404NotFound,
            ErrorCode = ApiErrorCodes.NotFound,
            ErrorCLRType = GetType().FullName,
            Message = Message
        };
    }

    private static string FormatErrorMessage(object? resourceId, string? resourceType)
    {
        var subject = resourceType ?? "Resource";

        return resourceId is not null
            ? $"{subject} `{resourceId}` was not found."
            : $"{subject} was not found.";
    }
}
