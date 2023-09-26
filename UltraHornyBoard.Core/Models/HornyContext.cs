using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using UltraHornyBoard.Shared;

namespace UltraHornyBoard.Core.Models;

public class HornyContext : DbContext
{
    private readonly AppSettings.DatabaseSettings configuration;


    public DbSet<Image> Images { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    public HornyContext(DbContextOptions<HornyContext> options, IOptions<AppSettings> configuration) : base(options)
    {
        this.configuration = configuration.Value.Database;
        ChangeTracker.StateChanged += OnEntityStateChanged;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(GetDbConnectionString());
    }

    private void OnEntityStateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        if (e.NewState == EntityState.Modified && e.Entry.Entity is BaseEntity entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var entitiesWithDate = modelBuilder.Model.GetEntityTypes()
            .Where(type => type.ClrType.IsSubclassOf(typeof(BaseEntity)))
            .ToList();

        foreach (var entity in entitiesWithDate)
        {
            modelBuilder.Entity(entity.ClrType)
                .Property("CreatedAt")
                .HasDefaultValueSql("NOW()");

            modelBuilder.Entity(entity.ClrType)
                .Property("UpdatedAt")
                .HasDefaultValueSql("NOW()");
        }

        base.OnModelCreating(modelBuilder);
    }

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

    private Exception ConfigError(string what, string configKey, string env)
    {
        return new KeyNotFoundException($"{what} is not set. Set {configKey} in appsettings.json or {env} environment variable.");
    }
}
