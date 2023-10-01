using System.Diagnostics.CodeAnalysis;
using UltraHornyBoard.Auth.Models;
using UltraHornyBoard.Core.Models;

namespace UltraHornyBoard.Core.Dto;

public class ExtendedUserInfo : BasicUserInfo
{
    public ExtendedUserInfo()
    {
    }

    [SetsRequiredMembers]
    public ExtendedUserInfo(User user) : base(user)
    {
        Email = user.Email;
    }

    public required string? Email { get; init; }
}
