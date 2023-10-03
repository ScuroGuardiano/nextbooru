using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UltraHornyBoard.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class ImageController : ControllerBase
{
    [Authorize]
    public IActionResult Index()
    {
        return NoContent();
    }
}

