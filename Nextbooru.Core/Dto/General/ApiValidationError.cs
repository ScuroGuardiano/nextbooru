using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nextbooru.Shared;

namespace Nextbooru.Core.Dto.General;

public class ApiValidationError : ApiErrorResponse
{
    public ModelStateDictionary? ModelState { get; init; }
}
