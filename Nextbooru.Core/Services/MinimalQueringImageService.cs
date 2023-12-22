using Dapper;
using Nextbooru.Auth.Models;
using Nextbooru.Core.Dto;
using Nextbooru.Core.Dto.Requests;
using Nextbooru.Core.Dto.Responses;
using Nextbooru.Core.Models;

namespace Nextbooru.Core.Services;

/// <summary>
/// Minimal Quering Image Service is similar to <see cref="ImageService"/>
/// but it's only for data quering and it's designed load and return
/// only needed data by the frontend in optimized manner.
/// <br/><br/>
/// For example <see cref="ImageService.ListImagesAsync"/> on ImageService
/// loads from database full <see cref="Models.Image"/> entities
/// and returns list with full <see cref="ImageDto"/>.
/// MinimalQueringImageService on the other hand with it's <see cref="MinimalQueringImageService.ListImagesAsync"/>
/// will load columns that are absolutely needed from the database and return list of <see cref="MinimalImageDto"/><br/>
/// <br/><br/>
/// Nextbooru is built to be the fastest booru ever created so this approach is needed to optimize data querying.
/// </summary>
public class MinimalQueringImageService
{
    private readonly AppDbContext dbContext;
    private readonly ImageService imageService;

    public MinimalQueringImageService(AppDbContext dbContext, ImageService imageService)
    {
        this.dbContext = dbContext;
        this.imageService = imageService;
    }

    public async Task<ListResponse<MinimalImageDto>> ListImagesAsync(ListImagesQuery request, User? user)
    {
        ArgumentNullException.ThrowIfNull(request.ResultsOnPage);

        List<int> tagsIdsFromQuery = [];
        if (request.TagsArray.Length > 0)
        {
            // TODO: [MINOR PERFORMANCE ISSUE] Can't we get this from somewhere else? Like Redis or maybe force frontend to query by tags id
            tagsIdsFromQuery = await dbContext.Tags
                .Where(t => request.TagsArray.Contains(t.Name))
                .Select(t => t.Id)
                .ToListAsync();
        }
        
        var toSkip = (request.Page - 1) * request.ResultsOnPage;
        var connection = dbContext.Database.GetDbConnection();
        var results = (await connection.QueryAsync<MinimalListImageModel>(
            """
                SELECT
                    images.id AS id,
                    images.uploaded_by_id,
                    images.title,
                    images.width,
                    images.height,
                    images.is_public,
                    images.extension,
                    ARRAY_AGG(tags.name) AS tags
                FROM images
                    -- I need to get rid of this JOINs, maybe query tag names in separate query
                    -- As those JOINs makes query to run like 15x slower on small dataset (500 matched, 8000 total in database)
                INNER JOIN image_tag
                    ON images.id = image_tag.images_id
                INNER JOIN tags
                    ON image_tag.tags_id = tags.id
                WHERE images.tags_arr @> @TagsIds
                    AND (images.is_public = true OR @IsAdmin = true OR (@UserId IS NOT NULL AND @UserId = images.uploaded_by_id))
                GROUP BY
                    images.id, uploaded_by_id, title, width, height, is_public, extension
                ORDER BY images.id DESC
                OFFSET @ToSkip
                LIMIT @ResultsOnPage
            """,
            new
            {
                TagsIds = tagsIdsFromQuery,
                IsAdmin = user?.IsAdmin,
                UserId = user?.Id,
                ToSkip = toSkip,
                ResultsOnPage = request.ResultsOnPage
            }
        )).ToList();
        
        // TODO: [CRITICAL PERFORMANCE ISSUE] counting on large datasets with filter applied is unacceptably slow.
        // Or maybe I tested it on wrong query this query is but there will be more complicated queries so I must test that then
        var total = await connection.QueryFirstAsync<int>(
            $"""
                SELECT COUNT(*) FROM images
                WHERE images.tags_arr @> @TagsIds
                AND (images.is_public = true OR @IsAdmin = true OR (@UserId IS NOT NULL AND @UserId = images.uploaded_by_id))
            """,
            new
            {
                TagsIds = tagsIdsFromQuery,
                IsAdmin = user?.IsAdmin,
                UserId = user?.Id,
            }
        );
        
        var totalPages = (int)Math.Ceiling((decimal)total / request.ResultsOnPage);

        return new ListResponse<MinimalImageDto>()
        {
            Data = results.Select(i => new MinimalImageDto()
            {
                Id = i.Id,
                Tags = i.Tags ?? [],
                Title = i.Title,
                Url = imageService.GetUrlForImage(i.Id, i.Extension ?? ""),
                ThumbnailUrl = imageService.GetThumbnailUrl(i.Id),
                IsPublic = i.IsPublic,
                Height = i.Height,
                Width = i.Width
            }).ToList(),
            Page = request.Page,
            TotalPages = totalPages,
            TotalRecords = total,
            RecordsPerPage = request.ResultsOnPage,
            LastRecordId = results.LastOrDefault()?.Id ?? 0
        };
    }
}

file sealed class MinimalListImageModel
{
    public long Id { get; set; }
    public Guid UploadedById { get; set; }
    public string? Title { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool IsPublic { get; set; }
    public string? Extension { get; set; }
    public string[]? Tags { get; set; }
}
