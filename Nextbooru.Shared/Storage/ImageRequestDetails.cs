namespace Nextbooru.Shared.Storage;

/// <summary>
/// Represents desired image details from Media Store with image conversion capabilities.
/// </summary>
public class ImageRequestDetails
{
    /// <summary>
    /// Desired image width, if zero then it should be ignored by MediaStore backend
    /// </summary>
    public uint Width { get; init; }
    
    /// <summary>
    /// Desired image height, if zero then it should be ignored by MediaStore backend
    /// </summary>
    public uint Height { get; init; }
    
    /// <summary>
    /// Desired image quality, range from 1 to 100. If it's zero then it should be ignored by MediaStore backend
    /// Image quality may be interpreted differently by different backends and for different file formats
    /// </summary>
    public uint Quality { get; init; }

    /// <summary>
    /// Desired file type. File type match file extension, e.g. `webp`, `png`, `jpg`
    /// </summary>
    public string? FileType { get; init; }
}
