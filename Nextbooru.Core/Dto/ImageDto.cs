using Nextbooru.Core.Models;

namespace Nextbooru.Core.Dto;

public class ImageDto
{
    public required long Id { get; init; }
    public required string Url { get; init; }
    public required string ThumbnailUrl { get; init; }
    public string? Title { get; init; }
    public string? Source { get; init; }
    public string? ContentType { get; init; }
    public string? Extension { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
    public long SizeInBytes { get; init; }
    public required bool IsPublic { get; init; }
    public int Score { get; set; }
    
    /// <summary>
    ///  -1 -> Downvoted, 1 -> Upvoted, 0 -> Unvoted
    /// </summary>
    public VoteScore? UserVote { get; set; }
    public List<TagDto> Tags { get; init; }
    public BasicUserInfo? UploadedBy { get; init; }

    public static ImageDto FromImageModel(Image image, string imageUrl, string thumbnailUrl)
    {
        return new ImageDto()
        {
            Id = image.Id,
            Url = imageUrl,
            ThumbnailUrl = thumbnailUrl,
            Title = image.Title,
            Source = image.Source,
            ContentType = image.ContentType,
            Extension = image.Extension,
            Width = image.Width,
            Height = image.Height,
            SizeInBytes = image.SizeInBytes,
            IsPublic = image.IsPublic,
            Score = image.Score,
            Tags = image.Tags?.Select(TagDto.FromTagModel).ToList() ?? [],
            UploadedBy = image.UploadedBy is not null ? new BasicUserInfo(image.UploadedBy) : null
        };
    }
}
