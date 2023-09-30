namespace UltraHornyBoard.Shared;

public class ApiErrorReponse
{
    public int StatusCode { get; init; }

    public string? ErrorType { get; init; }

    public string? Message { get; init; }
}
