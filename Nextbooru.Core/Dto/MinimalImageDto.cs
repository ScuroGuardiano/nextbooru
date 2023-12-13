using Nextbooru.Core.Models;

namespace Nextbooru.Core.Dto;

/// <summary>
/// Serves the same purpouse as <see cref="ImageDto"/> but
/// contains as little data as possible to display image list element on frontend.
/// Tags are stored as string list instead of <see cref="TagDto"/> list.
/// </summary>
public class MinimalImageDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public bool IsPublic { get; set; }
    public required string Url { get; set; }
    public required string ThumbnailUrl { get; set; }
    public required IEnumerable<string> Tags { get; set; }

    public static MinimalImageDto FromImageModel(Image image, string imageUrl, string thumbnailUrl)
    {
        return new MinimalImageDto
        {
            Id = image.Id,
            Title = image.Title,
            Height = image.Height,
            Width = image.Width,
            IsPublic = image.IsPublic,
            Url = imageUrl,
            ThumbnailUrl = thumbnailUrl,
            Tags = image.Tags?.Select(t => t.Name).ToList() ?? []
        };
    }
}
