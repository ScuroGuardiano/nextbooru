using Microsoft.EntityFrameworkCore;
using Nextbooru.Auth.Models;

namespace Nextbooru.Auth;

public static class AuthHelpers
{
    public static void RegisterAuthRelations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Session>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .IsRequired();

        modelBuilder.Entity<RolePermission>()
            .HasKey(rp => new { rp.RoleName, rp.PermissionKey });

        modelBuilder.Entity<RolePermission>()
            .HasOne(rp => rp.Role)
            .WithMany(r => r.Permissions)
            .HasForeignKey(rp => rp.RoleName);

        modelBuilder.Entity<UserPermission>()
            .HasKey(up => new { up.UserId, up.PermissionKey });

        modelBuilder.Entity<UserPermission>()
            .HasOne(up => up.User)
            .WithMany(u => u.Permissions)
            .HasForeignKey(up => up.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRole>(
                l => l.HasOne(ur => ur.Role).WithMany().HasForeignKey(ur => ur.RoleName),
                r => r.HasOne(ur => ur.User).WithMany().HasForeignKey(ur => ur.UserId)
            );
    }
}
