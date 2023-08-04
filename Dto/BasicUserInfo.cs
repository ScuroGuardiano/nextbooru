using System.Diagnostics.CodeAnalysis;
using UltraHornyBoard.Models;

namespace UltraHornyBoard.Dto;


//
// Summary:
//  Basic user info Dto, safe to show to everyone
public class BasicUserInfo
{
    public BasicUserInfo()
    {
    }

    [SetsRequiredMembers]
    public BasicUserInfo(User user)
    {
        (Username, DisplayName, IsAdmin) = (user.Username, user.DisplayName, user.IsAdmin);
    }

    public required string Username { get; init; }
    public required string DisplayName { get; init; }

    public required bool IsAdmin { get; init; }
}
