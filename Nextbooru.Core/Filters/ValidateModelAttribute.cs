using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Nextbooru.Shared;

namespace Nextbooru.Core.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            context.Result = new JsonResult(new Dto.ApiValidationError {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorCode = ApiErrorCodes.ValidationError,
                Message = "Incorrent entity was posted with request, more information in ModelState property.",
                ModelState = context.ModelState
            });
        }
    }
}
