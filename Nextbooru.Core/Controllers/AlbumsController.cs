using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nextbooru.Core.Dto.Requests;
using Nextbooru.Core.Dto.Responses;
using Nextbooru.Core.Models;
using Nextbooru.Shared.QueryHelpers;

namespace Nextbooru.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class AlbumsController : ControllerBase
{
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
    public ActionResult<AlbumDto> Create([FromBody] CreateAlbumRequest request)
    {
        throw new NotImplementedException();
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
