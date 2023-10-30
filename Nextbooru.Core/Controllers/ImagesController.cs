using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nextbooru.Auth.Models;
using Nextbooru.Auth.Services;
using Nextbooru.Core.Dto;
using Nextbooru.Core.Exceptions;
using Nextbooru.Core.Services;

namespace Nextbooru.Core.Controllers;

[ApiController]
[Route("[controller]")]
public class ImagesController : ControllerBase
{
    private readonly ImageService imageService;
    private readonly ISessionService<Session> sessionService;
    private readonly AppSettings configuration;

    public ImagesController(ImageService imageService, ISessionService<Session> sessionService, IOptions<AppSettings> options)
    {
        this.imageService = imageService;
        this.sessionService = sessionService;
        this.configuration = options.Value;
    }

    [HttpGet]
    public async Task<ListResponse<ImageDto>> ListImages([FromQuery] ListImagesQuery imagesQuery)
    {
        imagesQuery.ResultsOnPage ??= configuration.DefaultResultsPerPage;
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

        if (image.IsPublic)
        {
            return new JsonResult(ImageDto.FromImageModel(image, imageService.GetUrlForImage(image)));
        }
        
        // If image is not public only uploader and admin can see it
        
        var user = sessionService.GetCurrentSessionFromHttpContext()?.User;
        if (user is null || (!user.IsAdmin && !user.Id.Equals(image.UploadedById)))
        {
            return new StatusCodeResult(StatusCodes.Status403Forbidden);
        }
        
        return new JsonResult(ImageDto.FromImageModel(image, imageService.GetUrlForImage(image)));
    }

    /// <summary>
    /// Returns image file data. Later I'll add conversions here based on extension and dimensions.<br/>
    /// If we want our Nextbooru instance to be fast then this route shouldn't be used.
    /// Later I'll add maybe here some redirection based on config to dedicated static files server like for example nginx
    /// or some custom image server with conversion support written for example in Blazingly Fast Rust :3
    /// </summary>
    /// <param name="id"></param>
    /// <param name="extension"></param>
    /// <exception cref="NotFoundException"></exception>
    [HttpGet("{id:long}.{extension}")]
    public async Task GetImageFile([FromRoute] long id, [FromRoute] string extension)
    {
        var image = await imageService.GetImageAsync(id, includeUploadedBy: true);
        if (image is null)
        {
            throw new NotFoundException(id, "Image");
        }

        await using var imageStream = await imageService.GetImageStreamByFileIdAsync(image.StoreFileId);

        Response.ContentType = image.ContentType;
        
        if (image.IsPublic)
        {
            await imageStream.CopyToAsync(Response.Body);
            return;
        }
        
        // If image is not public only uploader and admin can see it
        var user = sessionService.GetCurrentSessionFromHttpContext()?.User;
        if (user is null || (!user.IsAdmin && !user.Id.Equals(image.UploadedById)))
        {
            Response.ContentType = "text/html";
            Response.StatusCode = StatusCodes.Status418ImATeapot;
            await Response.WriteAsync("<center><h1>418 - I'm a Teapot</h1><hr><p>Nextbooru refuses to show your desired image because it is, permanently, a teapot.</p></center>");
            return;
        }
        
        await imageStream.CopyToAsync(Response.Body);
    }
}
