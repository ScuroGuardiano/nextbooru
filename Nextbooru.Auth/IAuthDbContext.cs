using Microsoft.EntityFrameworkCore;
using Nextbooru.Auth.Models;

namespace Nextbooru.Auth;

public interface IAuthDbContext
{
    DbSet<User> Users { get; set; }
    DbSet<Session> Sessions { get; set; }
}
