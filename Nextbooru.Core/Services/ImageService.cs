using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Options;
using MimeTypes;
using Nextbooru.Auth.Models;
using Nextbooru.Core.Dto;
using Nextbooru.Core.Exceptions;
using Nextbooru.Core.Models;
using Nextbooru.Shared.Storage;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using ImageSharpImage = SixLabors.ImageSharp.Image;
using Image = Nextbooru.Core.Models.Image;

namespace Nextbooru.Core.Services;

public class ImageService
{
    private readonly AppDbContext dbContext;
    private readonly IMediaStore mediaStore;
    private readonly AppSettings configuration;
    private readonly ILogger<ImageService> logger;
    private readonly ImageConvertionService imageConvertionService;

    public ImageService(AppDbContext dbContext,
        IMediaStore mediaStore,
        IOptions<AppSettings> options,
        ILogger<ImageService> logger,
        ImageConvertionService imageConvertionService)
    {
        this.dbContext = dbContext;
        this.mediaStore = mediaStore;
        this.logger = logger;
        this.imageConvertionService = imageConvertionService;
        this.configuration = options.Value;
    }

    public string GetUrlForImage(Image image)
    {
        return GetUrlForImage(image.Id, image.Extension ?? "");
    }
    public string GetUrlForImage(long imageId, string extension)
    {
        // TODO: This HAS to be changed, it's terrible.
        return $"/api/images/{imageId}{extension}";
    }

    public string GetThumbnailUrl(Image image)
    {
        return GetThumbnailUrl(image.Id);
    }
    public string GetThumbnailUrl(long imageId)
    {   
        var format = configuration.Images.Thumbnails.Format;
        // TODO: This HAS to be changed, it's terrible.
        return $"/api/images/{imageId}.{format}";
    }

    public async Task<ListResponse<ImageDto>> ListImagesAsync(ListImagesQuery request, User? user)
    {
        ArgumentNullException.ThrowIfNull(request.ResultsOnPage);

        var toSkip = (request.Page - 1) * request.ResultsOnPage;
        var query = dbContext.Images.Include(i => i.Tags).AsQueryable();

        if (user is null)
        {
            query = query.Where(i => i.IsPublic == true);
        }

        if (user is not null && !user.IsAdmin)
        {
            query = query.Where(i => i.IsPublic == true || i.UploadedById == user.Id);
        }

        if (request.TagsArray.Length > 0)
        {
            // TODO: [MINOR PERFORMANCE ISSUE] Can't we get this from somewhere else?
            var tagsIdsFromQuery = await dbContext.Tags
                .Where(t => request.TagsArray.Contains(t.Name))
                .Select(t => t.Id)
                .ToListAsync();

            query = query.Where(i => tagsIdsFromQuery.All(id => i.TagsArr.Contains(id)));
        }

        // TODO: [CRITICAL PERFORMANCE ISSUE] counting on large datasets with filter applied is unacceptably slow.
        var total = await query.CountAsync();

        var results = await query
            .OrderByDescending(i => i.Id)
            .Skip(toSkip)
            .Take(request.ResultsOnPage)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling((decimal)total / request.ResultsOnPage);

        return new ListResponse<ImageDto>()
        {
            Data = results.Select(i => ImageDto.FromImageModel(i, GetUrlForImage(i), GetThumbnailUrl(i))).ToList(),
            Page = request.Page,
            TotalPages = totalPages,
            TotalRecords = total,
            RecordsPerPage = request.ResultsOnPage,
            LastRecordId = results.LastOrDefault()?.Id ?? 0
        };
    }

    public async Task<Image?> GetImageAsync(long id, bool includeTags = false, bool includeUploadedBy = false)
    {
        var query = dbContext.Images.Where(i => i.Id == id);

        if (includeTags)
        {
            query = query.Include(i => i.Tags);
        }

        if (includeUploadedBy)
        {
            query = query.Include(i => i.UploadedBy);
        }

        return await query.FirstOrDefaultAsync();
    }

    /// <summary>
    /// Returns image thumbnail stream with stored thumbnail having widths closes to desired.
    /// If no thumbnail is found in store it will convert original image in flight and produce warning
    /// as it's very CPU intensive task and should be avoided.
    /// </summary>
    /// <exception cref="UnsupportedMediaTypeException">
    /// When thumbnail format is not supported.
    /// </exception> 
    /// <returns></returns>
    public async Task<(Stream stream, string contentType)> GetImageThumbnailAsync(long imageId, int width, string format)
    {
        var thumbnailVariant = await dbContext.ImageVariants
            .Where(iv => iv.Extension == "." + format && iv.ImageId == imageId)
            .OrderBy(iv => Math.Abs(width - iv.Width))
            .FirstOrDefaultAsync();
        
        if (thumbnailVariant is not null)
        {
            return (
                await mediaStore.GetFileStreamAsync(thumbnailVariant.StoreFileId),
                thumbnailVariant.ContentType ?? MimeTypeMap.GetMimeType("." + format)
            );
        }

        if (!format.Equals(configuration.Images.Thumbnails.Format, StringComparison.CurrentCultureIgnoreCase))
        {
            throw new UnsupportedMediaTypeException($"Format {format} for thumbnail is not supported.");
        }

        logger.LogWarning(
            "Stored thumbnail for {imageId} was not found. Thumbnail will be generated in flight. This should be avoided as it's very CPU intensive, review you configuration, make sure you set thumbnails width to generate then run thumbnails generation task.",
            imageId
        );

        await using var originalImageStream = await GetImageStreamByIdAsync(imageId);
        if (originalImageStream is null)
        {
            throw new NotFoundException(imageId, "Image");
        }

        var ms = new MemoryStream();
        await imageConvertionService.ConvertImageAsync(
            originalImageStream,
            ms,
            new ImageConvertionOptions
            {
                Format = format.ToLower(),
                Quality = configuration.Images.Thumbnails.Quality,
                Width = width
            }
        );
        return (ms, MimeTypeMap.GetMimeType("." + format));
    }

    public async Task<Stream?> GetImageStreamByIdAsync(long id)
    {
        var imageFileId = await dbContext.Images
            .Where(i => i.Id == id)
            .Select(i => i.StoreFileId)
            .FirstOrDefaultAsync();

        if (imageFileId is null)
        {
            return null;
        }

        return await GetImageStreamByFileIdAsync(imageFileId);
    }

    /// <summary>
    /// TODO: TEMPORARY IMPLEMENTATION, IT'S NOT FINAL SOLUTION
    /// </summary>
    /// <param name="id"></param>
    /// <param name="width"></param>
    /// <param name="imageMode"></param>
    /// <param name="extension"></param>
    /// <returns></returns>
    /// <exception cref="UnsupportedContentTypeException"></exception>
    public async Task<Stream?> GetResizedImageStreamByIdAsync(long id, int width, string imageMode, string extension)
    {
        var stream = await GetImageStreamByIdAsync(id);
        var resultStream = new MemoryStream();
        if (stream is null)
        {
            return null;
        }
        var image = await ImageSharpImage.LoadAsync(stream);

        if (width < image.Width)
        {
            image.Mutate(x => x.Resize(width, 0));
        }

        if (!extension.StartsWith("."))
        {
            extension = "." + extension;
        }

        switch (extension)
        {
            case ".png":
                await image.SaveAsPngAsync(resultStream);
                break;

            case ".jpg":
            case ".jpeg":
                await image.SaveAsJpegAsync(resultStream);
                break;

            case ".gif":
                await image.SaveAsGifAsync(resultStream);
                break;

            case ".webp":
                await image.SaveAsWebpAsync(resultStream);
                break;

            default:
                throw new UnsupportedContentTypeException($"Image extension {extension} is not supported");
        }

        resultStream.Position = 0;
        return resultStream;
    }

    public async Task<Stream> GetImageStreamByFileIdAsync(string fileId)
    {
        return await mediaStore.GetFileStreamAsync(fileId);
    }

    public async Task<Image> AddImageAsync(UploadFileRequest request, Guid userId)
    {
        var fileExtension = MimeTypeMap.GetExtension(request.File.ContentType, false);

        if (!configuration.AllowedUploadExtensions.Contains(fileExtension))
        {
            throw new NotAllowedFileTypeException(fileExtension, request.File.ContentType);
        }
        
        int width = 0, height = 0;
        var size = request.File.Length;
        ImageSharpImage? rawImage = null;

        try
        {
            if (configuration.Images.Thumbnails.Widths.Count > 0)
            {
                rawImage = await LoadImageAsync(request);
                if (rawImage is not null)
                {
                    width = rawImage.Width;
                    height = rawImage.Height;
                }
            }
            else
            {
                (width, height) = await IdentifyAsync(request);
            }


            await using var fileStream = request.File.OpenReadStream();
            var storeFileId = await mediaStore.SaveFileAsync(fileStream, fileExtension);
            var tags = await GetOrAddTagsFromRequestAsync(request);

            var image = new Image()
            {
                StoreFileId = storeFileId,
                Title = request.Title,
                Source = request.Source,
                ContentType = request.File.ContentType,
                Extension = fileExtension,
                UploadedById = userId,
                Width = width,
                Height = height,
                SizeInBytes = size,
                Tags = tags,
                TagsArr = tags.Select(t => t.Id).ToList()
            };

            dbContext.Add(image);
            await dbContext.SaveChangesAsync();

            if (rawImage is not null)
            {
                await AddThumbnailsAsync(image, rawImage);
            }

            return image;
        }
        finally
        {
            rawImage?.Dispose();
        }

    }

    public Task<bool> CanUserReadImageAsync(User? user, Image image)
    {
        return Task.FromResult(image.IsPublic
                               || user is not null && (user.IsAdmin || user.Id == image.UploadedById)
        );
    }

    public async Task<bool> CanUserReadImageAsync(User? user, long imageId)
    {
        var userId = user?.Id ?? Guid.Empty;
        if (user is not null && user.IsAdmin)
        {
            return true;
        }

        return await dbContext.Images.AnyAsync(i => i.IsPublic || userId == i.UploadedById);
    }

    private async Task<List<ImageVariant>> AddThumbnailsAsync(Image image, ImageSharpImage rawImage)
    {
        List<ImageVariant> variants = [];

        foreach (var thumbnailWidth in configuration.Images.Thumbnails.Widths)
        {
            // TODO: [Performance Issue] saving first to MemoryStream and then to a file seems like a resource waste
            // Maybe media store should return writable stream to save file?
            using var ms = new MemoryStream();
            var (width, height) = await imageConvertionService.ConvertImageAsync(rawImage, ms, new()
            {
                Format = configuration.Images.Thumbnails.Format,
                Quality = configuration.Images.Thumbnails.Quality,
                Width = thumbnailWidth
            });
            ms.Position = 0;
            var size = ms.Length;
            var fileId = await mediaStore.SaveFileAsync(ms, $".{configuration.Images.Thumbnails.Format}");

            var variant = new ImageVariant() {
                ImageId = image.Id,
                StoreFileId = fileId,
                ContentType = MimeTypeMap.GetMimeType($".{configuration.Images.Thumbnails.Format}"),
                Extension = $".{configuration.Images.Thumbnails.Format}",
                Width = width,
                Height = height,
                SizeInBytes = size,
                VariantMode = VariantMode.Thumbnail
            };
            variants.Add(variant);
        }

        dbContext.AddRange(variants);
        await dbContext.SaveChangesAsync();
        return variants;
    }

    private async Task<List<Tag>> GetOrAddTagsFromRequestAsync(UploadFileRequest request, bool shouldIncreaseImagesCount = false)
    {
        var rawTags = request.Tags.Split(" ").Select(t => t.ToLower());

        var tags = await dbContext.Tags
            .Where(t => rawTags.Contains(t.Name))
            .ToListAsync();

        foreach (var rawTag in rawTags)
        {
            if (!tags.Exists(t => t.Name == rawTag))
            {
                var tag = new Tag() { Name = rawTag };
                tags.Add(tag);
                dbContext.Add(tag);
            }
        }

        if (shouldIncreaseImagesCount)
        {
            foreach (var tag in tags)
            {
                tag.ImagesCount++;
            }
        }

        await dbContext.SaveChangesAsync();

        return tags;
    }

    private async Task<(int width, int height)> IdentifyAsync(UploadFileRequest request)
    {
        int width = 0, height = 0;

        try
        {
            await using var fileStream = request.File.OpenReadStream();
            var imageInfo = await ImageSharpImage.IdentifyAsync(fileStream);
            imageInfo.Size.Deconstruct(out width, out height);
        }
        catch (Exception exception)
        {
            logger.LogError(
                "File {request.File.FileName} (Content-Type: {request.File.ContentType} is not a valid image. Exception: {exception}",
                request.File.FileName,
                request.File.ContentType,
                exception);

            if (configuration.StrictImageChecks)
            {
                throw new InvalidImageFileException();
            }
        }

        return (width, height);
    }

    private async Task<ImageSharpImage?> LoadImageAsync(UploadFileRequest request)
    {
        try
        {
            await using var fileStream = request.File.OpenReadStream();
            var image = await ImageSharpImage.LoadAsync(fileStream);
            return image;
        }
        catch (Exception exception)
        {
            logger.LogError(
                "File {request.File.FileName} (Content-Type: {request.File.ContentType} is not a valid image. Exception: {exception}",
                request.File.FileName,
                request.File.ContentType,
                exception);

            if (configuration.StrictImageChecks)
            {
                throw new InvalidImageFileException();
            }
        }
        return null;
    }
}
