using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Nextbooru.Core.Controllers;

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

