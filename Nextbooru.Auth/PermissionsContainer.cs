namespace Nextbooru.Auth;

public class PermissionsContainer
{
    private readonly Dictionary<string, Permission> permissions = [];

    public void RegisterPermission(Permission permission)
    {
        if (permissions.ContainsKey(permission.Key))
        {
            throw new InvalidOperationException($"Permission {permission.Key} is already registered.");
        }
        permissions.Add(permission.Key, permission);
    }

    public void RegisterPermissions(IEnumerable<Permission> permissions)
    {
        foreach (var permission in permissions)
        {
            RegisterPermission(permission);
        }
    }

    public IReadOnlyList<Permission> ListPermissions()
    {
        return permissions.Select(p => p.Value).ToList();
    }

    public Permission? GetPermission(string key)
    {
        return permissions[key];
    }
}
