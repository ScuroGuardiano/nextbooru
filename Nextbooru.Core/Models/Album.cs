using Nextbooru.Auth.Models;
using Nextbooru.Shared;
using Nextbooru.Shared.QueryHelpers;

namespace Nextbooru.Core.Models;

public class Album : BaseEntity
{
    [Orderable]
    public long Id { get; set; }
    
    [Required, Orderable]
    public required string Name { get; set; }
    
    public string? Description { get; set; }

    [Orderable]
    public bool IsPublic { get; set; }
    
    [Orderable]
    public int ImageCount { get; set; }
    
    [Orderable]
    public DateTime? PublishedAt { get; set; }
    
    public User? CreatedBy { get; set; }
    
    public Guid CreatedById { get; set; }

    public List<Image>? Images { get; set; } = [];
}
