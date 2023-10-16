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

    public async Task<ListResponse<ImageDto>> ListImagesAsync(ListImagesQuery request, User? user)
    {
        ArgumentNullException.ThrowIfNull(request.ResultsOnPage);

        var toSkip = (request.Page - 1) * request.ResultsOnPage.Value;
        var query = dbContext.Images.Include(i => i.Tags).AsQueryable();
        
        Console.WriteLine(user?.Username);
        
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
            // query = query.Where(
            //     i => request.TagsArray.AsQueryable().All(rawTag => i.Tags.Exists(t => t.Name == rawTag))
            //     );
            // TODO: Propably inefficent, test some versions, find a better way uwu
            query = query.Where(i => i.Tags.Count(t => request.TagsArray.Contains(t.Name)) == request.TagsArray.Length);
        }
        
        var total = await query.CountAsync();
        var results = await query.Skip(toSkip)
            .Take(request.ResultsOnPage.Value)
            .OrderByDescending(i => i.CreatedAt)
            .ToListAsync();
        var totalPages = (int)Math.Ceiling((decimal)total / request.ResultsOnPage.Value);

        return new ListResponse<ImageDto>()
        {
            Data = results.Select(ImageDto.FromImageModel).ToList(),
            Page = request.Page,
            TotalPages = totalPages,
            TotalRecords = total,
            RecordsPerPage = request.ResultsOnPage.Value
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
    
    public async Task<List<Image>> ListRecentlyAddedImagesAsync()
    {
        return await dbContext.Images
            .Where(x => x.IsPublic)
            .OrderBy(x => x.CreatedAt)
            .Take(20)
            .ToListAsync();
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
        var tags = await GetTagsFromRequestAsync(request);

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
            Tags = tags
        };

        dbContext.Add(image);
        await dbContext.SaveChangesAsync();

        return image;
    }

    private async Task<List<Tag>> GetTagsFromRequestAsync(UploadFileRequest request)
    {
        var rawTags = request.Tags.Split(" ").Select(t => t.ToLower());

        var tags = await dbContext.Tags
            .Where(t => rawTags.Contains(t.Name))
            .ToListAsync();

        foreach (var rawTag in rawTags)
        {
            if (!tags.Exists(t => t.Name == rawTag))
            {
                tags.Add(new Tag() { Name = rawTag });
            }
        }

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
