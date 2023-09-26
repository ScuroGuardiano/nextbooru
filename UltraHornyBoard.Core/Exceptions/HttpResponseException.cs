namespace UltraHornyBoard.Core.Exceptions;

public class HttpResponseException : Exception
{
    public HttpResponseException(int statusCode, string? errorType = null, string? message = null) : base(message) =>
        (StatusCode, ErrorType) = (statusCode, errorType);

    public int StatusCode { get; }

    public string? ErrorType { get; }
}
