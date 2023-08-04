using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UltraHornyBoard.Dto;

public class ApiValidationError : ApiError
{
    public ModelStateDictionary? ModelState { get; init; }
}
