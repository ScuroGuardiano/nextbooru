using Microsoft.AspNetCore.Mvc;
using Nextbooru.Core.Dto;
using Nextbooru.Core.Services;

namespace Nextbooru.Core.Controllers;

[ApiController]
[Route("tags")]
public class TagsController : ControllerBase
{
    private readonly TagsService tagsService;
    
    public TagsController(TagsService tagsService)
    {
        this.tagsService = tagsService;
    }

    [HttpGet("autocomplete")]
    public async Task<List<TagDto>> Autocomplete([FromQuery] string phrase)
    {
        return await tagsService.Autocomplete(phrase);
    }
}
