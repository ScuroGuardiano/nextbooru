using System.Diagnostics.CodeAnalysis;
using UltraHornyBoard.Models;

namespace UltraHornyBoard.Dto;

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

    public required string Email { get; init; }
}
