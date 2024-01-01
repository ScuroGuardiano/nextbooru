using Microsoft.EntityFrameworkCore;
using Nextbooru.Auth.Models;
using Nextbooru.Shared.Exceptions;

namespace Nextbooru.Auth.Services;

public class AuthorizationManager<TDbContext> : IAuthorizationManager
    where TDbContext : DbContext, IAuthDbContext
{
    private TDbContext dbContext;

    public AuthorizationManager(TDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Role?> GetRoleAsync(string roleName)
    {
        return await dbContext.Roles.FirstOrDefaultAsync(role => role.Name == roleName);
    }

    public async Task<Role> CreateRoleAsync(string roleName)
    {
        var existingRole = await GetRoleAsync(roleName);

        if (existingRole is not null)
        {
            throw new RecordAlreadyExistsException(roleName, "Role");
        }

        var role = new Role() { Name = roleName };
        dbContext.Add(role);
        await dbContext.SaveChangesAsync();

        return role;
    }

    public async Task AddPermissionToRoleAsync(string roleName, string permissionKey)
    {
        try
        {
            var rolePermission = new RolePermission()
            {
                RoleName = roleName,
                PermissionKey = permissionKey
            };

            dbContext.Add(rolePermission);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            // TODO: translate exception from database about
            // violation of relation constraint (role not existing) to HTTP 404
            // duplicated record to HTTP 409 - Conflict
            throw;
        }
    }

    public async Task AddUserToRoleAsync(Guid userId, string roleName)
    {
        try
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null)
            {
                throw new NotFoundException(userId, "User");
            }

            var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName);

            if (role is null)
            {
                throw new NotFoundException(roleName, "Role");
            }

            role.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            // TODO: Translate exception from database about
            // violation of unique index constraint (user is already in role) to HTTP 409 - Conflict
            throw;
        }
    }

    public async Task AddPermissionToUserAsync(Guid userId, string permissionKey)
    {
        try
        {
            var userPermission = new UserPermission()
            {
                UserId = userId,
                PermissionKey = permissionKey
            };

            dbContext.Add(userPermission);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception)
        {
            // TODO: Translate exception from database about
            // violation of relation constraint (user not existing) to HTTP 404
            // violation of primary key constraint (duplicated record) to 409 - Conflict
            throw;
        }
    }

    public async Task<bool> DoesUserHasPermissionAsync(Guid userId, string permissionKey)
    {
        return await (from user in dbContext.Users
                      from userPermission in user.Permissions
                      from role in user.Roles
                      from rolePermission in role.Permissions
                      where user.Id == userId && (user.IsAdmin ||
                        userPermission.PermissionKey == permissionKey
                          || rolePermission.PermissionKey == permissionKey)
                      select user).AnyAsync();
    }
}
