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
        if (context.Exception is null)
        {
            return;
        }

        if (!ApiErrorResponseConverter.TryFromException(context.Exception, out var apiErrorResponse))
        {
            return;
        }
        
        context.Result = new JsonResult(apiErrorResponse)
        {
            StatusCode = apiErrorResponse.StatusCode
        };
            
        context.ExceptionHandled = true;

    }
}
