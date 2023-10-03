using System.Diagnostics.CodeAnalysis;
using Nextbooru.Auth.Models;
using Nextbooru.Core.Models;

namespace Nextbooru.Core.Dto;

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
