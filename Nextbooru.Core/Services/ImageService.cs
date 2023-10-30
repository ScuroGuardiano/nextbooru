using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeTypes;
using Nextbooru.Auth.Models;
using Nextbooru.Core.Dto;
using Nextbooru.Core.Exceptions;
using Nextbooru.Core.Models;
using Nextbooru.Shared.Storage;
using ImageSharpImage = SixLabors.ImageSharp.Image;

namespace Nextbooru.Core.Services;

public class ImageService
{
    private readonly AppDbContext dbContext;
    private readonly IMediaStore mediaStore;
    private readonly AppSettings configuration;
    private readonly ILogger<ImageService> logger;

    public ImageService(AppDbContext dbContext,
        IMediaStore mediaStore,
        IOptions<AppSettings> options,
        ILogger<ImageService> logger)
    {
        this.dbContext = dbContext;
        this.mediaStore = mediaStore;
        this.logger = logger;
        this.configuration = options.Value;
    }

    public string GetUrlForImage(Image image)
    {
        // TODO: This HAS to be changed, it's terrible.
        return $"/api/images/{image.Id}{image.Extension}";
    }
    
    public async Task<ListResponse<ImageDto>> ListImagesAsync(ListImagesQuery request, User? user)
    {
        ArgumentNullException.ThrowIfNull(request.ResultsOnPage);

        var toSkip = (request.Page - 1) * request.ResultsOnPage.Value;
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
        
        // TODO: [CRITICAL PERFORMANCE ISSUE] counting on large datasets with filter applies is unacceptably slow.
        var total = await query.CountAsync();
        
        var results = await query
            .OrderByDescending(i => i.Id)
            .Skip(toSkip)
            .Take(request.ResultsOnPage.Value)
            .ToListAsync();
        
        var totalPages = (int)Math.Ceiling((decimal)total / request.ResultsOnPage.Value);

        return new ListResponse<ImageDto>()
        {
            Data = results.Select(i => ImageDto.FromImageModel(i, GetUrlForImage(i))).ToList(),
            Page = request.Page,
            TotalPages = totalPages,
            TotalRecords = total,
            RecordsPerPage = request.ResultsOnPage.Value,
            LastRecordId = results.Last().Id
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

        var (width, height) = await IdentifyAsync(request);
        var size = request.File.Length;

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

        return image;
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
}
