using FluentValidation;

namespace Nextbooru.Core.Dto;

public class UploadFileRequest
{
    [Required]
    public required IFormFile File { get; set; }
    
    [Required]
    public required string Tags { get; set; }

    public string? Title { get; set; }
    
    public string? Source { get; set; }

    public class UploadFileRequestValidator : AbstractValidator<UploadFileRequest>
    {
        public UploadFileRequestValidator()
        {
            RuleFor(x => x.File).NotNull();
            // No other tags validations here, those will be handled by a parser
            RuleFor(x => x.Tags).NotEmpty();
            RuleFor(x => x.Title).MaximumLength(128);
            RuleFor(x => x.Source).MaximumLength(2048);
        }
    }
}
