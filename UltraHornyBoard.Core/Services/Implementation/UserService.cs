using System.Net;
using UltraHornyBoard.Core.Dto;
using UltraHornyBoard.Core.Exceptions;
using UltraHornyBoard.Core.Models;
using BC = BCrypt.Net.BCrypt;

namespace UltraHornyBoard.Core.Services.Implementation;

public class UserService : IUserService
{
    private readonly HornyContext context;

    public UserService(HornyContext context)
    {
        this.context = context;
    }

    public async Task<User> CreateUser(UserRegisterRequest userData)
    {
        var displayName = userData.Username;
        var username = userData.Username.ToLower();
        var email = userData.Email.ToLower();

        if (await DoesUserAlreadyExists(username, email))
        {
            throw new HttpResponseException(
                (int)HttpStatusCode.Conflict,
                "UserAlreadyExists",
                "User already exists."
            );
        }

        var hashedPassword = BC.HashPassword(userData.Password, 12);

        var user = new User() {
            Username = username,
            DisplayName = displayName,
            HashedPassword = hashedPassword,
            Email = email
        };

        context.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task ChangePassword(string currentPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    public async Task<User> MakeAdmin(string username)
    {
        var user = await context.Users
            .Where(user => user.Username == username)
            .FirstAsync();
        user.IsAdmin = false;
        await context.SaveChangesAsync();
        return user;
    }

    private async Task<bool> DoesUserAlreadyExists(string username, string email)
    {
        var user = await context.Users
            .Where(user => 
                user.Username == username ||
                user.Email == email
            )
            .FirstOrDefaultAsync();
        return user is not null;
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await context.Users
            .Where(user => user.Id == id)
            .FirstOrDefaultAsync();
    }
}
