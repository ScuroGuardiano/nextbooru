using FluentValidation;

namespace Nextbooru.Core.Dto;

public class UserLoginRequest
{
    public required string Username { get; init; }
    public required string Password { get; init; }
    
    public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
    {
        public UserLoginRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

}
