using Nextbooru.Shared;

namespace Nextbooru.Core.Models;

public class Tag : BaseEntity
{
    [Key]
    [Required]
    public required string Name { get; set; }

    [Required]
    public TagType TagType { get; set; } = TagType.General;

    public List<Image> Images { get; } = new();
}
