using FluentValidation;
using SGLibCS.Utils.Validation;

namespace Nextbooru.Core.Dto;

public class UserRegisterRequest
{
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
    
    public class UserRegisterRequestValidator : AbstractValidator<UserRegisterRequest>
    {
        public UserRegisterRequestValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .Length(3, 16)
                .Must(BeNotOneOf(["me", "admin"])).WithMessage("Username {PropertyValue} is forbidden.")
                .Matches("^[a-zA-Z0-9]+[a-zA-Z0-9_]*[a-zA-Z0-9]$");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .Length(8, 72);
        }

        private Func<string, bool> BeNotOneOf(string[] forbiddenValues)
        {
            return (string x) => !forbiddenValues.Contains(x);
        }
    }

}
