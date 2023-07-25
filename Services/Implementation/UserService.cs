using System.Net;
using Microsoft.AspNetCore.Mvc;
using UltraHornyBoard.Dto;
using UltraHornyBoard.Exceptions;
using UltraHornyBoard.Models;
using UltraHornyBoard.Services;
using BCrypt.Net;

namespace UltraHornyBoard.Services.Implementation;

public class UserService : IUserService
{
    private readonly HornyContext context;

    public UserService(HornyContext context)
    {
        this.context = context;
    }

    public async Task<User> CreateUser(RegisterUser userData)
    {
        // Sanity checks, this actually should never happen, coz dto is validated.
        ArgumentNullException.ThrowIfNullOrEmpty(userData.Username);
        ArgumentNullException.ThrowIfNullOrEmpty(userData.Email);
        ArgumentNullException.ThrowIfNullOrEmpty(userData.Password);

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

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userData.Password, 12);

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

    private async Task<bool> DoesUserAlreadyExists(string username, string email)
    {
        var user = await context.Users
            .Where(user => 
                user.Username == username ||
                user.Email == email
            )
            .FirstAsync();
        return user is not null;
    }
}