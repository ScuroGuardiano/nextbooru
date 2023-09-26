using UltraHornyBoard.Core.Exceptions;

namespace UltraHornyBoard.Core.Dto;

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
