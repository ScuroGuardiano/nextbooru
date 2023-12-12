namespace Nextbooru.Core.Models.QueryTypes;

[Keyless]
public sealed class MinimalListImageModel
{
    public long Id { get; set; }
    public Guid UploadedById { get; set; }
    public string? Title { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool IsPublic { get; set; }
    public string? Extension { get; set; }
    public List<string>? Tags { get; set; }
}
