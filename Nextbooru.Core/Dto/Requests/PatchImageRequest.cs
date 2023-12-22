using FluentValidation;

namespace Nextbooru.Core.Dto.Requests;

public class PatchImageRequest
{
    public string? Tags { get; set; }
    public string? Title { get; set; }
    public string? Source { get; set; }

    public class PatchImageRequestValidator : AbstractValidator<PatchImageRequest>
    {
        public PatchImageRequestValidator()
        {
            RuleFor(x => x.Title).MaximumLength(128);
            RuleFor(x => x.Source).MaximumLength(2048);
        }
    }
}
