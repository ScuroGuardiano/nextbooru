using Microsoft.EntityFrameworkCore;
using UltraHornyBoard.Auth.Models;

namespace UltraHornyBoard.Auth;

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
