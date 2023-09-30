namespace UltraHornyBoard.Shared;

public interface IConvertibleToApiErrorResponse
{
    ApiErrorReponse ToApiErrorResponse();
    Task<ApiErrorReponse> ToApiErrorResponseAsync() {
        return Task.FromResult(ToApiErrorResponse());
    }
}
