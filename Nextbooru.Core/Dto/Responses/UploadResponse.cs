namespace Nextbooru.Core.Dto.Responses;

public class UploadResponse
{
    public required string ImageUrl { get; init; }
    public required string ImageMetadataUrl { get; init; }
}
