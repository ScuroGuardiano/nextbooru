using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Nextbooru.Auth.Models;
using Nextbooru.Auth.Services;
using Nextbooru.Core.Dto.Requests;
using Nextbooru.Core.Dto.Responses;
using Nextbooru.Core.Models;
using Nextbooru.Core.Services;
using Nextbooru.Shared.QueryHelpers;

namespace Nextbooru.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class AlbumsController : ControllerBase
{
    private readonly AlbumService albumService;
    private readonly ISessionService sessionService;

    public AlbumsController(AlbumService albumService, ISessionService sessionService)
    {
        this.albumService = albumService;
        this.sessionService = sessionService;
    }

    [HttpGet]
    public ListResponse<AlbumDto> List([FromQuery] ListAlbumsQuery request)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id:long}")]
    public ActionResult<AlbumDto> Get([FromRoute] long id)
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("{id:long}/images")]
    public ListResponse<MinimalImageDto> ListImagesInAlbum([FromRoute] long id, [FromQuery] ListImagesQuery request)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost]
    [DefaultStatusCode(StatusCodes.Status201Created)]
    public async Task<AlbumDto> Create([FromBody] CreateAlbumRequest request)
    {
        var userId = sessionService.GetCurrentSessionFromHttpContext()!.UserId;
        var res = AlbumDto.From(await albumService.CreateAlbum(userId, request));
        return res;
    }

    [Authorize]
    [HttpPut]
    public ActionResult<AlbumDto> Rename([FromBody] RenameAlbumRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpGet("orderable-fields")]
    public IEnumerable<string> GetTagOrderableFields()
    {
        return QueryHelper.GetOrderableFields<Album>();
    }
}
