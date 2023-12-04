using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nextbooru.Core.Dto;
using Nextbooru.Core.Models;
using Nextbooru.Core.Services;
using Nextbooru.Shared.QueryHelpers;

namespace Nextbooru.Core.Controllers;

[ApiController]
[Route("tags")]
public class TagsController : ControllerBase
{
    private readonly TagsService tagsService;
    private readonly AppSettings configuration;
    
    public TagsController(TagsService tagsService, IOptions<AppSettings> options)
    {
        this.tagsService = tagsService;
        this.configuration = options.Value;
    }

    [HttpGet("autocomplete")]
    public async Task<List<TagDto>> Autocomplete([FromQuery] string phrase)
    {
        return await tagsService.Autocomplete(phrase);
    }

    [HttpGet("orderable-fields")]
    public IEnumerable<string> GetTagOrderableFields()
    {
        return QueryHelper.GetOrderableFields<Tag>();
    }

    [HttpGet]
    public async Task<ListResponse<TagDto>> ListTags([FromQuery] ListTagsQuery query)
    {
        query.ResultsOnPage ??= configuration.DefaultResultsPerPage;
        if (query.ResultsOnPage > configuration.MaxResultsPerPage)
        {
            query.ResultsOnPage = configuration.MaxResultsPerPage;
        }
        
        return await tagsService.ListTags(query);
    }
}
