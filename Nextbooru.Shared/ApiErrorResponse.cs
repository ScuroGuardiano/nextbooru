namespace Nextbooru.Shared;

public class ApiErrorReponse
{
    public int StatusCode { get; init; }

    public string? ErrorCode { get; init; }

    public string? ErrorCLRType { get; init; }

    public string? Message { get; init; }
}
