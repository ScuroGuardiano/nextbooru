using System.Diagnostics.CodeAnalysis;
using Nextbooru.Auth.Models;

namespace Nextbooru.Core.Dto.Responses;

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
