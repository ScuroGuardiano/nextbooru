namespace UltraHornyBoard.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UltraHornyBoard.Dto;
using UltraHornyBoard.Exceptions;

public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
{
    // The preceding filter specifies an Order of the maximum integer value minus 10. This Order allows other filters to run at the end of the pipeline.
    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception is HttpResponseException exception)
        {
            context.Result = new JsonResult(ApiError.FromHttpResponseException(exception))
            {
                StatusCode = exception.StatusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
