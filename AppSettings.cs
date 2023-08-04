namespace UltraHornyBoard;

using SGLibCS.Ms;

public class AppSettings
{
    public static Dictionary<string, string> EnvMappings { get; } = new () {
        { "DB_HOST", "UHB_DATABASE__HOST" },
        { "DB_PORT", "UHB_DATABASE__PORT" },
        { "DB_USERNAME", "UHB_DATABASE__USERNAME" },
        { "DB_PASSWORD", "UHB_DATABASE__PASSWORD" },
        { "DB_DATABASE", "UHB_DATABASE__DATABASE" },
        { "JWT_KEY", "JWT__KEY" }
    };

    public static string EnvPrefix { get; } = "UHB_";

    public AppInfoSettings? AppInfo { get; set; }
    
    [Required, ValidateObject]
    public required DatabaseSettings Database { get; set; }

    [Required, ValidateObject]
    public required JwtSettings Jwt { get; set; }

    public bool DisableRegistration { get; set; } = false;
    public bool DisableLogin { get; set; } = false;

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
        public string? AuthorURL { get; set; }
    }

    public class JwtSettings
    {
        [Required(ErrorMessage = "Jwt.Key is required. Set it via JWT_KEY environment variable.")]
        public required string Key { get; set; }

        public string Issuer { get; set; } = "UltraHornyBoard";

        public string Audience { get; set; } = "UltraHornyBoard";

        [MsFormat]
        public string JwtExpiration { get; set; } = "1h";

        public TimeSpan JwtExpirationTS { get => MsConverter.ParseToTimeSpan(JwtExpiration); }
    }
}
