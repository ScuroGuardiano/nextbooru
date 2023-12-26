using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Nextbooru.Auth.Dto;
using Nextbooru.Auth.Exceptions;
using Nextbooru.Auth.Models;

namespace Nextbooru.Auth.Services;

public class UserService<TDbContext> : IUserService
    where TDbContext : DbContext, IAuthDbContext
{
    private readonly TDbContext dbContext;

    public UserService(TDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<User> AuthenticateUser(LoginUserRequest dto)
    {
        var usernameLower = dto.Username.ToLowerInvariant();

        var user = await dbContext.Users
            .Where(user => user.Username == usernameLower)
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

    public async Task<User?> GetById(Guid id)
    {
        return await dbContext.Users
            .Where(user => user.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetById(string id)
    {
        var success = Guid.TryParse(id, out var guid);
        return success ? await GetById(guid) : null;
    }

    public async Task<User> RegisterUserAsync(RegisterUserRequest dto)
    {
        if (await DoesUserAlreadyExits(dto))
        {
            throw new UserAlreadyExistsException(dto.Username);
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password, 11);
        var user = new User {
            Username = dto.Username!.ToLowerInvariant(),
            DisplayName = dto.Username,
            Email = dto.Email,
            HashedPassword = hashedPassword
        };

        dbContext.Add(user);
        await dbContext.SaveChangesAsync();
        return user;
    }

    public ClaimsPrincipal UserToClaimsPrincipal(User user, string authScheme)
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
        var lowerUsername = dto.Username.ToLowerInvariant();
        return await dbContext.Users
            .Where(user => user.Username == lowerUsername)
            .AnyAsync();
    }
}
