using System.Security.Claims;
using Nextbooru.Auth.Dto;
using Nextbooru.Auth.Models;

namespace Nextbooru.Auth.Services;

public interface IUserService<TUser>
where TUser : User
{
    Task<TUser> RegisterUserAsync(RegisterUserRequest dto);
    Task<TUser> AuthenticateUser(LoginUserRequest dto);
    ClaimsPrincipal UserToClaimsPrincipal(TUser user, string authScheme);

    Task<TUser?> GetById(Guid id);
    Task<TUser?> GetById(string id);
}
