using Nextbooru.Core.Exceptions;

namespace Nextbooru.Core.Dto;

public class ApiError
{
    public int StatusCode { get; init; }

    public string? ErrorType { get; init; }

    public string? Message { get; init; }

    public static ApiError FromHttpResponseException(HttpResponseException exception)
    {
        return new ApiError
        {
            StatusCode = exception.StatusCode,
            ErrorType = exception.ErrorType,
            Message = exception.Message
        };
    }
}
