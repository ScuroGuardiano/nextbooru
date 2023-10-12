namespace Nextbooru.Core.Dto;

public class UploadFileRequest
{
    [Required]
    public required IFormFile File { get; set; }
    
    [Required]
    public required string Tags { get; set; }

    public string? Title { get; set; }
    
    public string? Source { get; set; }
}
