using FluentValidation;

namespace Nextbooru.Core.Dto.Requests;

public class UpdateImageRequest
{
    public required string Tags { get; set; }
    public string? Title { get; set; }
    public string? Source { get; set; }

    public class UpdateImageRequestValidator : AbstractValidator<UpdateImageRequest>
    {
        public UpdateImageRequestValidator()
        {
            RuleFor(x => x.Tags).NotNull();
            RuleFor(x => x.Title).MaximumLength(128);
            RuleFor(x => x.Source).MaximumLength(2048);
        }
    }
}
