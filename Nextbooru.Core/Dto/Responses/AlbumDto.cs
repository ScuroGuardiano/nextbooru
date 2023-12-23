using Nextbooru.Core.Models;

namespace Nextbooru.Core.Dto.Responses;

public class AlbumDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsPublic { get; set; }
    public int ImageCount { get; set; }
    public DateTime? PublishedAt { get; set; }
    public BasicUserInfo? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public static AlbumDto From(Album album)
    {
        return new AlbumDto()
        {
            Id = album.Id,
            Name = album.Name,
            Description = album.Description,
            IsPublic = album.IsPublic,
            ImageCount = album.ImageCount,
            PublishedAt = album.PublishedAt,
            CreatedBy = album.CreatedBy is not null ? new BasicUserInfo(album.CreatedBy) : null,
            CreatedAt = album.CreatedAt,
            UpdatedAt = album.UpdatedAt
        };
    }
}
