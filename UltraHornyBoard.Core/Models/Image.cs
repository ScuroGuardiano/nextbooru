using UltraHornyBoard.Shared;

namespace UltraHornyBoard.Core.Models;

public class Image : BaseEntity
{
    public long Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public string? Url { get; set; }
}
