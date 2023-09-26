using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UltraHornyBoard.Core.Dto;

public class ApiValidationError : ApiError
{
    public ModelStateDictionary? ModelState { get; init; }
}
