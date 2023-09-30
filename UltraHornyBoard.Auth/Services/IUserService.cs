using System.Security.Claims;
using UltraHornyBoard.Auth.Dto;
using UltraHornyBoard.Auth.Models;

namespace UltraHornyBoard.Auth.Services;

public interface IUserService<TUser>
where TUser : User
{
    Task<TUser> RegisterUserAsync(RegisterUserRequest dto);
    Task<TUser> AuthenticateUser(LoginUserRequest dto);
    ClaimsPrincipal UserToClaimsPrincipal(TUser user, string authScheme);

    Task<TUser?> GetById(Guid id);
    Task<TUser?> GetById(string id);
}
