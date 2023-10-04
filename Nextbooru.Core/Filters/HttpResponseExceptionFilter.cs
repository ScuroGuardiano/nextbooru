namespace Nextbooru.Core.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nextbooru.Core.Exceptions;
using Nextbooru.Shared;

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    // The preceding filter specifies an Order of the maximum integer value minus 10. This Order allows other filters to run at the end of the pipeline.
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is not HttpResponseException exception)
        {
            // I don't know if I need this, for now I'll let ASP.NET to handle uknown exceptions

            // var env = context.HttpContext.RequestServices.GetService<IWebHostEnvironment>();
            // string? clrType = null;
            // string message = "Internal server error.";
            
            // // Uknown exceptions can include some sensitive data
            // // I want to send it with response only if app is in development mode.
            // if (env?.IsDevelopment() ?? false)
            // {
            //     clrType = context.Exception?.GetType().FullName;
            //     message = context.Exception?.Message ?? message;
            // }

            // var apiErrorResponse = new ApiErrorReponse
            // {
            //     Message = message,
            //     ErrorCLRType = clrType,
            //     ErrorCode = ApiErrorCodes.InternalServerError,
            //     StatusCode = StatusCodes.Status500InternalServerError
            // };

            // context.Result = new JsonResult(apiErrorResponse)
            // {
            //     StatusCode = StatusCodes.Status500InternalServerError
            // };
            return;
        }
        
        context.Result = new JsonResult(exception.ToApiErrorResponse())
        {
            StatusCode = exception.StatusCode
        };

        context.ExceptionHandled = true;
    }
}
