using Nextbooru.Auth.Models;
using Nextbooru.Shared;

namespace Nextbooru.Core.Models;

public class Album : BaseEntity
{
    public long Id { get; set; }
    
    [Required]
    public required string Name { get; set; }
    
    public string? Description { get; set; }

    public bool IsPublic { get; set; }
    
    public int ImageCount { get; set; }
    
    public DateTime? PublishedAt { get; set; }
    
    public User? CreatedBy { get; set; }
    
    public Guid CreatedById { get; set; }

    public List<Image>? Images { get; set; } = [];
}
