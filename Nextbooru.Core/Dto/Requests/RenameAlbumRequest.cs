using FluentValidation;

namespace Nextbooru.Core.Dto.Requests;

public class RenameAlbumRequest
{
    public required string Name { get; set; }

    public class RenameAlbumRequestValidator : AbstractValidator<RenameAlbumRequest>
    {
        public RenameAlbumRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(128);
        }
    }
}
