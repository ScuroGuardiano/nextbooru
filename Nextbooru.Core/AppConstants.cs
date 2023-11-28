namespace Nextbooru.Core;

public static class AppConstants
{
    public static string? DefaultMediaStoragePath
    {
        get
        {
            if (OperatingSystem.IsWindows())
            {
                return $@"{Environment.GetEnvironmentVariable("APPDATA")}\Nextbooru\storage";
            }
            if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
            {
                return $"{Environment.GetEnvironmentVariable("HOME")}/.nextbooru/storage";
            }
            if (OperatingSystem.IsMacOS())
            {
                throw new Exception("Fuck Apple, no default media storage path for apple, cya.");
            }

            return null;
        }
    }

    public static class ImageModes
    {
        public const string Thumbnail = "thumbnail";
        public const string Convert = "convert";
        public const string Raw = "raw";
    }
}
