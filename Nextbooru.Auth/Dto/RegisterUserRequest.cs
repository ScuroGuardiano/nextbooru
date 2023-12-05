using FluentValidation;

namespace Nextbooru.Auth.Dto;

public class RegisterUserRequest
{
    public required string Username { get; set; }
    public string? Email { get; set; }
    public required string Password { get; set; }

    public class RegisterUserRequestValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .Length(3, 16)
                .Must(NotBeOneOf(["me", "admin"]))
                .WithMessage("Username {PropertyValue} is forbidden.")
                .Matches("^[a-zA-Z0-9]+[a-zA-Z0-9_]*[a-zA-Z0-9]$");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(8, 72);
        }

        private Func<string, bool> NotBeOneOf(string[] forbiddenValues)
        {
            return x => !forbiddenValues.Contains(x);
        }
    }
}
