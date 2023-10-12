namespace Nextbooru.Shared;

public interface IConvertibleToApiErrorResponse
{
    ApiErrorResponse ToApiErrorResponse();
    Task<ApiErrorResponse> ToApiErrorResponseAsync() {
        return Task.FromResult(ToApiErrorResponse());
    }
}
