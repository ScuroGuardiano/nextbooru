namespace Nextbooru.Auth;

public static class AuthenticationConstants
{
    public static string AuthenticationScheme { get; } = "SGAuth";
    public static string SessionClaimType { get;} = "sg_session";
    public static string SessionHttpContextItemKey { get; } = "sg_session";
}
