using Microsoft.EntityFrameworkCore;

namespace Nextbooru.Auth.Models;

[Keyless]
public class AuthorizationMetadata
{
    public bool BuiltinRolesInitialized { get; set; }
}
