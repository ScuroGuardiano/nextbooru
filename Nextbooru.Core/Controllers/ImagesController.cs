using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nextbooru.Auth.Models;
using Nextbooru.Auth.Services;
using Nextbooru.Core.Dto;
using Nextbooru.Core.Dto.Requests;
using Nextbooru.Core.Dto.Responses;
using Nextbooru.Core.Exceptions;
using Nextbooru.Core.Models;
using Nextbooru.Core.Services;

namespace Nextbooru.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class ImagesController : ControllerBase
{
    private readonly ImageService imageService;
    private readonly MinimalQueringImageService minimalImageService;
    private readonly ImageVotingService imageVotingService;
    private readonly ISessionService<Session> sessionService;
    private readonly AppSettings configuration;

    public ImagesController(
        ImageService imageService,
        ISessionService<Session> sessionService,
        IOptions<AppSettings> options, ImageVotingService imageVotingService, MinimalQueringImageService minimalImageService)
    {
        this.imageService = imageService;
        this.sessionService = sessionService;
        this.imageVotingService = imageVotingService;
        this.minimalImageService = minimalImageService;
        this.configuration = options.Value;
    }

    [HttpGet]
    public async Task<ListResponse<MinimalImageDto>> ListImages([FromQuery] ListImagesQuery imagesQuery)
    {
        if (imagesQuery.ResultsOnPage <= 0)
        {
            imagesQuery.ResultsOnPage = configuration.DefaultResultsPerPage;
        }
        
        if (imagesQuery.ResultsOnPage > configuration.MaxResultsPerPage)
        {
            imagesQuery.ResultsOnPage = configuration.MaxResultsPerPage;
        }

        var user = sessionService.GetCurrentSessionFromHttpContext()?.User;

        return await minimalImageService.ListImagesAsync(imagesQuery, user);
    }
    
    [HttpGet("detailed")]
    public async Task<ListResponse<ImageDto>> ListDetailedImages([FromQuery] ListImagesQuery imagesQuery)
    {
        if (imagesQuery.ResultsOnPage <= 0)
        {
            imagesQuery.ResultsOnPage = configuration.DefaultResultsPerPage;
        }
        
        if (imagesQuery.ResultsOnPage > configuration.MaxResultsPerPage)
        {
            imagesQuery.ResultsOnPage = configuration.MaxResultsPerPage;
        }

        var user = sessionService.GetCurrentSessionFromHttpContext()?.User;

        return await imageService.ListImagesAsync(imagesQuery, user);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetImageMetadata([FromRoute] long id)
    {
        var image = await imageService.GetImageAsync(id, includeTags: true, includeUploadedBy: true);
        if (image is null)
        {
            throw new NotFoundException(id, "Image");
        }
        
        var user = sessionService.GetCurrentSessionFromHttpContext()?.User;
        if (!image.IsPublic)
        {
            // If image is not public only uploader and admin can see it
            if (user is null || (!user.IsAdmin && !user.Id.Equals(image.UploadedById)))
            {
                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }

        var imageDto = ImageDto.FromImageModel(
            image,
            imageService.GetUrlForImage(image),
            imageService.GetThumbnailUrl(image)
        );

        imageDto.UserVote = user is not null ? await imageVotingService.GetUserVote(user.Id, image.Id) : null;
        
        return new JsonResult(imageDto);
    }

    /// <summary>
    /// Returns image file data. Later I'll add conversions here based on extension and dimensions.<br/>
    /// If we want our Nextbooru instance to be fast then this route shouldn't be used.
    /// Later I'll add maybe here some redirection based on config to dedicated static files server like for example nginx
    /// or some custom image server with conversion support written for example in Blazingly Fast Rust :3
    /// </summary>
    /// <exception cref="NotFoundException"></exception>
    [HttpGet("{id:long}.{format}")]
    public async Task GetImageFile([FromRoute] long id, [FromRoute] string format, [FromQuery] GetImageFileQuery query)
    {
        var image = await imageService.GetImageAsync(id, includeUploadedBy: true);
        if (image is null)
        {
            throw new NotFoundException(id, "Image");
        }

        if (!image.IsPublic)
        {
            // If image is not public only uploader and admin can see it
            var user = sessionService.GetCurrentSessionFromHttpContext()?.User;
            if (!imageService.CanUserReadImage(user, image))
            {
                Response.ContentType = "text/html";
                Response.StatusCode = StatusCodes.Status418ImATeapot;
                await Response.WriteAsync("<center><h1>418 - I'm a Teapot</h1><hr><p>Nextbooru refuses to show your desired image because it is, permanently, a teapot.</p></center>");
                return;
            }
        }

        switch (query.Mode.ToLower())
        {
            case AppConstants.ImageModes.Thumbnail:
                await SendThumbnailImage(image, format, query.W);
                return;
            case AppConstants.ImageModes.Convert:
                // TODO
                throw new NotImplementedException();
            default:
                await SendOriginalImage(image);
                return;
        }
    }
    
    [Authorize]
    [HttpPut("{id:long}/upvote")]
    public async Task<IActionResult> Upvote([FromRoute] long id)
    {
        var user = sessionService.GetCurrentSessionFromHttpContext()!.User!;
        
        if (!await imageService.CanUserReadImageAsync(user, id))
        {
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        return new JsonResult(await imageVotingService.SwitchUpvote(user.Id, id));
    }
    
    [Authorize]
    [HttpPut("{id:long}/downvote")]
    public async Task<IActionResult> Downvote([FromRoute] long id)
    {
        var user = sessionService.GetCurrentSessionFromHttpContext()!.User!;
        
        if (!await imageService.CanUserReadImageAsync(user, id))
        {
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        return new JsonResult(await imageVotingService.SwitchDownvote(user.Id, id));
    }

    [Authorize]
    [HttpPut("{id:long}/make-public")]
    public async Task<IActionResult> MakePublic([FromRoute] long id)
    {
        var user = sessionService.GetCurrentSessionFromHttpContext()!.User!;

        if (!await imageService.CanUserMakeImagePublicAsync(user, id))
        {
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        await imageService.MakeImagePublicAsync(id);

        return NoContent();
    }

    [Authorize]
    [HttpPut("{id:long}/make-non-public")]
    public async Task<IActionResult> MakeNonPublic([FromRoute] long id)
    {
        var user = sessionService.GetCurrentSessionFromHttpContext()!.User!;

        if (!await imageService.CanUserMakeImageNonPublicAsync(user, id))
        {
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }

        await imageService.MakeImageNonPublicAsync(id);

        return NoContent();
    }
    
    private async Task SendOriginalImage(Image image)
    {
        Response.ContentType = image.ContentType;
        await using var imageStream = await imageService.GetImageStreamByFileIdAsync(image.StoreFileId);
        await imageStream.CopyToAsync(Response.Body);
    }

    private async Task SendThumbnailImage(Image image, string format, int width)
    {
        var (stream, contentType) = await imageService.GetImageThumbnailAsync(image.Id, width, format);
        await using (stream)
        {
            Response.ContentType = contentType;
            await stream.CopyToAsync(Response.Body);
        }
    }
}
