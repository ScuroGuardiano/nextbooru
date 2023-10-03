using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Nextbooru.Core.Dto;

public class ApiValidationError : ApiError
{
    public ModelStateDictionary? ModelState { get; init; }
}
