namespace Nextbooru.Core.Dto;

public class UploadResponse
{
    public required string ImageUrl { get; init; }
    public required string ImageMetadataUrl { get; init; }
}
