using Microsoft.EntityFrameworkCore;

namespace Nextbooru.Auth;

public class AuthHelpers
{
    public static void RegisterSessionUserRelation<TUser, TSession>(ModelBuilder modelBuilder)
    where TUser : Models.User
    where TSession : Models.Session
    {
        modelBuilder.Entity<TSession>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .IsRequired();
    }
}
