using FluentValidation;

namespace Nextbooru.Core.Dto.Requests;

public class CreateAlbumRequest
{
    public required string Name { get; set; }
    public bool IsPublic { get; set; }
    public string? Description { get; set; }
    

    public class CreateAlbumRequestValidator : AbstractValidator<CreateAlbumRequest>
    {
        public CreateAlbumRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(128);

            RuleFor(x => x.Description)
                .MaximumLength(2048);
        }
    }
}
