using UltraHornyBoard.Services;

namespace UltraHornyBoard.Middlewares;

public class LoadUserMiddleware
{
    private readonly RequestDelegate _next;

    public LoadUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserService userService)
    {
        

        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }
}
