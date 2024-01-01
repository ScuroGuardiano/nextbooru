using Nextbooru.Auth.Models;

namespace Nextbooru.Auth.Services;

public interface IAuthorizationManager
{
    public Task<Role?> GetRoleAsync(string roleName);
    public Task<Role> CreateRoleAsync(string roleName);
    public Task AddPermissionToRoleAsync(string roleName, string permissionKey);
    public Task AddUserToRoleAsync(Guid userId, string roleName);
    public Task AddPermissionToUserAsync(Guid userId, string permissionKey);

    /// <summary>
    /// Checks if an user has a permission or is assigned to role containing that permission.
    /// <br/>
    /// <b>If an user is admin this method will always return true</b>
    /// </summary>
    public Task<bool> DoesUserHasPermissionAsync(Guid userId, string permissionKey);
}
