using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Nextbooru.Auth.Dto;
using Nextbooru.Auth.Exceptions;
using Nextbooru.Auth.Models;

namespace Nextbooru.Auth.Services;

public class UserService<TDbContext> : UserService<TDbContext, User, Session>
    where TDbContext : DbContext, IAuthDbContext
{
    public UserService(TDbContext dbContext)
        : base(dbContext)
    {
    }
}

public class UserService<TDbContext, TUser> : UserService<TDbContext, TUser, Session>
    where TDbContext : DbContext, IAuthDbContext<TUser, Session>
    where TUser : User, new()
{
    public UserService(TDbContext dbContext)
        : base(dbContext)
    {
    }
}

public class UserService<TDbContext, TUser, TSession> : IUserService<TUser>
    where TDbContext : DbContext, IAuthDbContext<TUser, TSession>
    where TSession : Session, new()
    where TUser : User, new()
{

    private readonly TDbContext dbContext;

    public UserService(TDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    
    public async Task<TUser> AuthenticateUser(LoginUserRequest dto)
    {
        var user = await dbContext.Users
            .Where(user => string.Equals(user.Username, dto.Username, StringComparison.CurrentCultureIgnoreCase))
            .FirstOrDefaultAsync();

        if (user is null)
        {
            throw new WrongUsernameOrPasswordException();
        }

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.HashedPassword))
        {
            throw new WrongUsernameOrPasswordException();
        }

        return user;
    }

    public async Task<TUser?> GetById(Guid id)
    {
        return await dbContext.Users
            .Where(user => user.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<TUser?> GetById(string id)
    {
        var success = Guid.TryParse(id, out var guid);
        return success ? await GetById(guid) : null;
    }

    public async Task<TUser> RegisterUserAsync(RegisterUserRequest dto)
    {
        if (await DoesUserAlreadyExits(dto))
        {
            throw new UserAlreadyExistsException(dto.Username);
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password, 11);
        var user = new TUser {
            Username = dto.Username!.ToLower(),
            DisplayName = dto.Username,
            Email = dto.Email,
            HashedPassword = hashedPassword
        };

        dbContext.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

    public ClaimsPrincipal UserToClaimsPrincipal(TUser user, string authScheme)
    {
        var claims = new List<Claim>() {
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        return new ClaimsPrincipal(
            new ClaimsIdentity(claims, authScheme)
        );
    }

    private async Task<bool> DoesUserAlreadyExits(RegisterUserRequest dto)
    {
        return await dbContext.Users
            .Where(user => string.Equals(user.Username, dto.Username!, StringComparison.CurrentCultureIgnoreCase))
            .AnyAsync();
    }
}
