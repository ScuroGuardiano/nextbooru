using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nextbooru.Auth.Models;
using Nextbooru.Auth.Services;
using Nextbooru.Core.Dto;
using Nextbooru.Core.Dto.Requests;
using Nextbooru.Core.Dto.Responses;
using Nextbooru.Core.Services;

namespace Nextbooru.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class UploadController : ControllerBase
{
    private readonly ImageService imageService;
    private readonly ISessionService<Session> sessionService;

    public UploadController(ImageService imageService, ISessionService<Session> sessionService)
    {
        this.imageService = imageService;
        this.sessionService = sessionService;
    }

    [Authorize]
    [HttpPost]
    [RequestSizeLimit(100L * 1024 * 1024)]
    public async Task<UploadResponse> Upload([FromForm] UploadFileRequest body)
    {
        var userId = sessionService.GetCurrentSessionFromHttpContext()!.UserId;
        var image = await imageService.AddImageAsync(body, userId);

        HttpContext.Response.StatusCode = StatusCodes.Status201Created;
        
        // TODO: Throw those magic strings somewhere.
        return new UploadResponse()
        {
            ImageUrl = $"/api/images/{image.Id}{image.Extension}",
            ImageMetadataUrl = $"/api/images/{image.Id}"
        };
    }
}
