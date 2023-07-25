using UltraHornyBoard.Exceptions;

namespace UltraHornyBoard.Dto;

public class ApiError
{
    public int StatusCode { get; set; }

    public string? ErrorType { get; set; } 

    public string? Message { get; set; }

    public static ApiError FromHttpResponseException(HttpResponseException exception) {
        return new ApiError {
            StatusCode = exception.StatusCode,
            ErrorType = exception.ErrorType,
            Message = exception.Message
        };
    }
}