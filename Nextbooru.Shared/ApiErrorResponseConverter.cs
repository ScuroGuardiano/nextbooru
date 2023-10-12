namespace Nextbooru.Shared;

public static class ApiErrorResponseConverter
{
    public static bool TryFromException(Exception exception, out ApiErrorResponse response)
    {
        if (exception is IConvertibleToApiErrorResponse convertible)
        {
            response = Convertible(convertible);
            return true;
        }

        response = null!;
        return false;
    }

    private static ApiErrorResponse Convertible(IConvertibleToApiErrorResponse exception)
    {
        return exception.ToApiErrorResponse();
    }
}
