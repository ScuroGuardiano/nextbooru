using System.Security.Claims;
using Nextbooru.Auth.Dto;
using Nextbooru.Auth.Models;

namespace Nextbooru.Auth.Services;

public interface IUserService
{
    Task<User> RegisterUserAsync(RegisterUserRequest dto);
    Task<User> AuthenticateUser(LoginUserRequest dto);
    ClaimsPrincipal UserToClaimsPrincipal(User user, string authScheme);

    Task<User?> GetById(Guid id);
    Task<User?> GetById(string id);
}
