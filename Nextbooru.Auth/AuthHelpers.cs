using Microsoft.EntityFrameworkCore;
using Nextbooru.Auth.Models;

namespace Nextbooru.Auth;

public static class AuthHelpers
{
    public static void RegisterSessionUserRelation(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Session>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .IsRequired();
    }
}
