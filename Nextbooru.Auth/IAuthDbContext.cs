using Microsoft.EntityFrameworkCore;
using Nextbooru.Auth.Models;

namespace Nextbooru.Auth;

public interface IAuthDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Session> Sessions { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<UserRole> UserRoles { get; set; }
    DbSet<RolePermission> RolePermissions { get; set; }
    DbSet<UserPermission> UserPermissions { get; set; }
}
