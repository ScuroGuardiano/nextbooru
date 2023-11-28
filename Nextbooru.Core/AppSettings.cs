namespace Nextbooru.Core;

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

    public ImagesSettings Images { get; set; } = new();

    public bool DisableRegistration { get; set; }
    public bool DisableLogin { get; set; }

    [Required]
    public string MediaStoragePath { get; set; } = AppConstants.DefaultMediaStoragePath!;

    // Są problemy z listami, bo wartości z appsettings.json są do nich dodawane zamiast zastępować listy, które winny być default XD
    // Muszę coś wymyślić na to, bo chcę mieć default wartości w kodzie.    
    public List<string> AllowedUploadExtensions { get; set; } = new() /* { ".jpg", ".jpeg", ".png", ".gif" }*/;
    
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

    public class ImagesSettings
    {
        public ThumbnailsSettings Thumbnails { get; set; } = new();
        public ConvertionSettings Convertion { get; set; } = new();

        public class ThumbnailsSettings
        {
            public string Format { get; set; } = "webp";
            public int Quality { get; set; } = 65;
            public List<int> Widths { get; set; } = new() /*{ 200, 300 }*/;
        }

        public class ConvertionSettings
        {
            public bool AllowConvertion { get; set; } = true;
            public List<string> AllowedFormats = new() /*{ "png", "jpg", "jpeg", "webp", "gif" }*/;
        }
    }
}
