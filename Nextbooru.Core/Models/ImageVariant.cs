using Nextbooru.Shared;

namespace Nextbooru.Core.Models;

public enum VariantMode
{
    Standard,
    Thumbnail,
    Preview
}

public class ImageVariant : BaseEntity
{
    public long Id { get; set; }

    public Image? Image { get; set; }

    public long ImageId { get; set; }

    [Required]
    public required string StoreFileId { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public string? Extension { get; set; }

    public string? ContentType { get; set; }

    public VariantMode VariantMode { get; set; }
    public long SizeInBytes { get; set; }
}