namespace Nextbooru.Auth;

#pragma warning disable CA1711

/// <summary>
/// Represents permission xD
/// </summary>
public class Permission
{
    public Permission(string key, string name)
    {
        Key = key;
        Name = name;
    }

    public Permission(string key, string name, string description)
    {
        Key = key;
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Key should follow rule: Project.Scope.Action<br/>
    /// For example: Nextbooru.Core.Images.Upload
    /// </summary>
    public required string Key { get; init; }

    /// <summary>
    /// Human readable permission name.
    /// </summary>
    public required string Name { get; init; }

    public string? Description { get; init; }
}
