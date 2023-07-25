using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace UltraHornyBoard.Models;

public class HornyContext : DbContext
{
    private AppSettings.DatabaseSettings configuration;

    public HornyContext(DbContextOptions<HornyContext> options, IOptions<AppSettings> configuration) : base(options)
    {
        this.configuration = configuration.Value.Database;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(GetDbConnectionString());
    }

    public DbSet<Image> Images { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    private string GetDbConnectionString()
    {
        string connectionString = "";

        var host = configuration.Host;
        if (host is null)
        {
            throw ConfigError("Database host", "Database.Host", "DB_HOST");
        }

        var port = configuration.Port ?? "5432";
        var username = configuration.Username;
        if (username is null)
        {
            throw ConfigError("Database user", "Database.Username", "DB_USER");
        }

        var password = configuration.Password;
        var database = configuration.Database;

        connectionString += $"Host={host};Username={username}";
        
        if (port is not null)
        {
            connectionString += $";Port={port}";
        }

        if (password is not null)
        {
            connectionString += $";Password={password}";
        }

        if (database is not null)
        {
            connectionString += $";Database={database}";
        }

        return connectionString;
    }

    private Exception ConfigError(string what, string configKey, string env) {
        return new KeyNotFoundException($"{what} is not set. Set {configKey} in appsettings.json or {env} environment variable.");
    }
}