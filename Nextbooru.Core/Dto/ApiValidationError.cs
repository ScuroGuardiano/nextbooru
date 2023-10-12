using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nextbooru.Shared;

namespace Nextbooru.Core.Dto;

public class ApiValidationError : ApiErrorResponse
{
    public ModelStateDictionary? ModelState { get; init; }
}
