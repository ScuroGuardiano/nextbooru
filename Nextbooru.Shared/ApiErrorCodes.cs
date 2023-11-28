namespace Nextbooru.Shared;

public static class ApiErrorCodes
{
    // Constants here are okey, coz I will never change them.
    public const string WrongUsernameOrPassword = "WrongUsernameOrPassword";
    public const string UserAlreadyExists = "UserAlreadyExists";

    public const string ValidationError = "ValidationError";

    public const string CurrentUserNotFound = "CurrentUserNotFound";

    public const string InternalServerError = "InternalServerError";
    public const string FileDoesNotExists = "FileDoesNotExists";
    public const string NotAllowedFileType = "NotAllowedFileType";

    public const string UnsupportedMediaType = "UnsupportedMediaType";
    public const string InvalidImageFile = "InvalidImageFile";
    public const string NotFound = "NotFound";
}
