using Microsoft.AspNetCore.Http;

namespace UltraHornyBoard.Shared;

public static class HttpHelpers
{
    public static string? GetRealRemoteIpAddress(HttpContext ctx)
    {
        string? ipAddress = ctx.Request.Headers["X-Forwarded-For"];
        return ipAddress?.Split(',').FirstOrDefault() ?? ctx.Connection.RemoteIpAddress?.ToString();
    }
}
