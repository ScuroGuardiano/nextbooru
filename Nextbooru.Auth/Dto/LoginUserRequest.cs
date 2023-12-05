using FluentValidation;

namespace Nextbooru.Auth.Dto;

public class LoginUserRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    
    public class LoginUserRequestValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
