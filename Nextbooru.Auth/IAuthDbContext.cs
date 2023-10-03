using Microsoft.EntityFrameworkCore;
using Nextbooru.Auth.Models;

namespace Nextbooru.Auth;

public interface IAuthDbContext : IAuthDbContext<User, Session>
{
}

public interface IAuthDbContext<TUser, TSession>
where TUser : User
where TSession : Session
{
    DbSet<TUser> Users { get; set; }
    DbSet<TSession> Sessions { get; set; }
}
