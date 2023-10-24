using System.Numerics;
using Nextbooru.Auth.Models;
using Nextbooru.Shared;

namespace Nextbooru.Core.Models;

public class Image : BaseEntity
{
    public long Id { get; set; }

    /// <summary>
    /// File ID in store. In LocalMediaStore it would be just filename.
    /// </summary>
    [Required]
    public required string StoreFileId { get; set; }
    
    public string? Title { get; set; }

    public string? Source { get; set; }
    
    public string? ContentType { get; set; }

    public string? Extension { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }
    
    public long SizeInBytes { get; set; }
    
    public List<Tag> Tags { get; set; } = new();

    public List<int> TagsArr { get; set; } = new();
    
    public User? UploadedBy { get; set; }
    
    [Required]
    public Guid UploadedById { get; set; }

    [Required]
    public bool IsPublic { get; set; }
}
