using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Nextbooru.Auth.Models;
using Nextbooru.Shared;

namespace Nextbooru.Core.Models;

public class Image : BaseEntity
{
    public Image() { }

    /// <summary>
    /// Constructor used for partial update ONLY.
    /// Annotation lies, it doesn't set required members,
    /// I just added it to make the compiler to shut up ^^
    /// </summary>
    /// <param name="id"></param>
    [SetsRequiredMembers]
    public Image(long id)
    {
        Id = id;
    }
    
    public long Id { get; set; }

    /// <summary>
    /// File ID in store. In LocalMediaStore it would be just filename.
    /// </summary>
    [Required]
    public required string StoreFileId { get; set; } = null!;

    public List<ImageVariant> Variants { get; set; } = [];
    
    public string? Title { get; set; }

    public string? Source { get; set; }
    
    public string? ContentType { get; set; }

    public string? Extension { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }
    
    public long SizeInBytes { get; set; }
    
    public List<Tag>? Tags { get; set; } = [];

    public List<int> TagsArr { get; set; } = [];

    public List<Album> Albums { get; set; } = [];
    
    public int Score { get; set; }
    
    public User? UploadedBy { get; set; }
    
    [Required]
    public Guid UploadedById { get; set; }

    [Required]
    public bool IsPublic { get; set; }
    
    public DateTime? PublishedAt { get; set; }
}
