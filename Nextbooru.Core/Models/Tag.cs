using Nextbooru.Shared;

namespace Nextbooru.Core.Models;

public class Tag : BaseEntity
{
    public int Id { get; set; }
    
    [Required]
    public required string Name { get; set; }

    [Required]
    public TagType TagType { get; set; } = TagType.General;
    
    /// <summary>
    /// Just information for an user. It should count only <b>public</b> images.
    /// </summary>
    public int ImagesCount { get; set; }

    public List<Image> Images { get; } = new();
}
