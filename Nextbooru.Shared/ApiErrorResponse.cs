namespace Nextbooru.Shared;

public class ApiErrorReponse
{
    // For easy check on frontend
    public string Type { get; } = "ApiErrorResponse";
    public int StatusCode { get; init; }

    public string? ErrorCode { get; init; }

    public string? ErrorCLRType { get; init; }

    public string? Message { get; init; }
}
