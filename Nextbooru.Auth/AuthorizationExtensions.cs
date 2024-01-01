using System.Collections.Immutable;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nextbooru.Auth.Authorization;
using Nextbooru.Auth.Models;
using Nextbooru.Auth.Services;

namespace Nextbooru.Auth;

public static class AuthorizationExtensions
{
    public static void AddSGAuthorization<TDbContext>(this IServiceCollection services, Action<SGAuthorizationBuilder> configure)
        where TDbContext : DbContext, IAuthDbContext, new()
    {
        var builder = new SGAuthorizationBuilder();
        var (permissions, roles) = builder.Build();

        permissionsContainer.RegisterPermissions(permissions);
        try
        {
            InitializeRoles<TDbContext>(roles);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Role initialization failed. You will have no builin roles!");
        }

        services.AddSingleton(permissionsContainer);
        services.AddScoped<IAuthorizationManager, AuthorizationManager<TDbContext>>();
        services.AddScoped<IAuthorizationHandler, HasPermissionHandler>();
    }

    public static async Task<AuthorizationResult> HasPermissionAsync(
        this IAuthorizationService authorizationService,
        ClaimsPrincipal User,
        string permissionKey)
    {
        return await authorizationService.AuthorizeAsync(User, null, new HasPermissionRequirement(permissionKey));
    }

    private static void InitializeRoles<TDbContext>(IEnumerable<Role> roles)
        where TDbContext : DbContext, IAuthDbContext, new()
    {
        logger.LogInformation("Adding builin roles...");
        using var context = new TDbContext();
        using var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);
        var authorizationMetadata = context.AuthorizationMetadata.First();

        if (authorizationMetadata is not null && authorizationMetadata.BuiltinRolesInitialized)
        {
            logger.LogInformation("Builin roles has been already initialized, skipping.");
            transaction.Commit();
            return;
        }

        foreach (var role in roles)
        {
            if (context.Roles.FirstOrDefault(r => r.Name == role.Name) is not null)
            {
                logger.LogInformation("Role {Name} already exists, skipping.", new { role.Name });
                continue;
            }
            context.Add(role);
            logger.LogInformation("Builin role {Name} will be inserted.", new { role.Name });
        }

        if (authorizationMetadata is null)
        {
            authorizationMetadata = new AuthorizationMetadata() { BuiltinRolesInitialized = true };
            context.Add(authorizationMetadata);
        }
        else
        {
            authorizationMetadata.BuiltinRolesInitialized = true;
        }

        logger.LogInformation("Saving information to database...");
        context.SaveChanges();
        transaction.Commit();
        logger.LogInformation("Builin roles has been initialized successfully!");
    }

    private static readonly PermissionsContainer permissionsContainer = new();
    private static readonly ILogger logger = LoggerFactory
        .Create(builder => builder.ClearProviders().AddConsole())
        .CreateLogger("SGAuthorization");
}

public class SGAuthorizationBuilder
{
    private readonly List<Permission> permissions = [];
    private readonly List<Role> builtinRoles = [];

    /// <summary>
    /// Add permission to the permission container.<br/>
    /// It will allow to list permissions with their descriptions and easily manage them
    /// on the frontend.
    /// </summary>
    public SGAuthorizationBuilder AddPermissions(ICollection<Permission> permissions)
    {
        this.permissions.AddRange(permissions);
        return this;
    }

    /// <summary>
    /// Adds builtin role to authorization. Builtin roles cannot be deleted and are added only
    /// at the first start of the app. It's good to define roles like "User" and "Anonymous"
    /// </summary>
    /// <param name="roleName">Name of role</param>
    /// <param name="permisisionKeys">Initial permissions of the role. They will be only added at the first start of the app.</param>
    ///
    public SGAuthorizationBuilder AddBuiltinRole(string roleName, ICollection<string> permisisionKeys)
    {
        var rolePermissions = permisisionKeys.Select(p => new RolePermission { RoleName = roleName, PermissionKey = p });
        var role = new Role { Name = roleName, Builtin = true };
        builtinRoles.Add(role);
        return this;
    }

    public (IImmutableList<Permission> permissions, IImmutableList<Role> roles) Build()
    {
        return (permissions.ToImmutableList(), builtinRoles.ToImmutableList());
    }
}

