namespace Nextbooru.Core;

using SGLibCS.Ms;
using SGLibCS.Utils.Validation;

public class AppSettings
{
    public static string EnvPrefix { get; } = "UHB_";
    
    public static Dictionary<string, string> EnvMappings { get; } = new () {
        { "DB_HOST", $"{EnvPrefix}DATABASE__HOST" },
        { "DB_PORT", $"{EnvPrefix}DATABASE__PORT" },
        { "DB_USERNAME", $"{EnvPrefix}DATABASE__USERNAME" },
        { "DB_PASSWORD", $"{EnvPrefix}DATABASE__PASSWORD" },
        { "DB_DATABASE", $"{EnvPrefix}DATABASE__DATABASE" }
    };

    public AppInfoSettings? AppInfo { get; set; }
    
    [Required, ValidateObject]
    public required DatabaseSettings Database { get; set; }

    public bool DisableRegistration { get; set; }
    public bool DisableLogin { get; set; }

    [Required]
    public string MediaStoragePath { get; set; } = AppConstants.DefaultMediaStoragePath!;
    
    public List<string> AllowedUploadExtensions { get; set; } = new() { ".jpg", ".jpeg", ".png", ".gif" };
    
    /// <summary>
    /// Strict image checks not only checks format of an image but also it's validity.
    /// If image is not valid then error is returned.
    /// </summary>
    public bool StrictImageChecks { get; set; }

    public int DefaultResultsPerPage { get; set; } = 20;

    public int MaxResultsPerPage { get; set; } = 100;
    
    // Subtypes

    public class DatabaseSettings
    {
        [Required(ErrorMessage = "Database.Host is required. Set it in appsettings.json or as DB_HOST environment variable")]
        public required string Host { get; set; }
        public string? Port { get; set; }

        [Required(ErrorMessage = "Database.Username is required. Set it in appsettings.json or as DB_USERNAME environment variable")]
        public required string Username { get; set; }
        public string? Password { get; set; }
        public string? Database { get; set; }
    }

    public class AppInfoSettings
    {
        public string? Version { get; set; }
        public string? Name { get; set; }
        public string? Author { get; set; }
        public string? AuthorUrl { get; set; }
    }
}
