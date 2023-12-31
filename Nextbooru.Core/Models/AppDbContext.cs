using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Nextbooru.Auth;
using Nextbooru.Shared;

namespace Nextbooru.Core.Models;

public sealed class AppDbContext : AuthDbContext
{
    private readonly AppSettings.DatabaseSettings configuration;


    public DbSet<Image> Images { get; set; } = null!;
    public DbSet<ImageVariant> ImageVariants { get; set; } = null!;
    public DbSet<ImageVote> ImageVotes { get; set; } = null!;
    public DbSet<Tag> Tags { get; set; } = null!;
    public DbSet<Album> Albums { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options, IOptions<AppSettings> configuration) : base(options)
    {
        this.configuration = configuration.Value.Database;
        ChangeTracker.StateChanged += OnEntityStateChanged;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(GetDbConnectionString())
            .UseSnakeCaseNamingConvention();
    }

    private void OnEntityStateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        if (e is { NewState: EntityState.Modified, Entry.Entity: BaseEntity entity })
        {
            entity.UpdatedAt = DateTime.UtcNow;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Image>()
            .HasOne(i => i.UploadedBy)
            .WithMany()
            .HasForeignKey(i => i.UploadedById)
            .IsRequired();

        modelBuilder.Entity<Image>()
            .HasIndex(i => i.UploadedById)
            .IsUnique(false);

        modelBuilder.Entity<Image>()
            .HasMany(i => i.Tags)
            .WithMany(t => t.Images);

        modelBuilder.Entity<Image>()
            .HasIndex(i => i.TagsArr)
            .HasMethod("GIN");

        modelBuilder.Entity<ImageVariant>()
            .HasOne(iv => iv.Image)
            .WithMany(i => i.Variants)
            .HasForeignKey(iv => iv.ImageId)
            .IsRequired();

        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();

        modelBuilder.Entity<ImageVote>()
            .HasOne(iv => iv.User)
            .WithMany()
            .HasForeignKey(iv => iv.UserId)
            .IsRequired();

        modelBuilder.Entity<ImageVote>()
            .HasOne(iv => iv.Image)
            .WithMany()
            .HasForeignKey(iv => iv.ImageId)
            .IsRequired();

        modelBuilder.Entity<ImageVote>()
            .HasIndex(iv => new { iv.ImageId, iv.UserId })
            .IsUnique();

        modelBuilder.Entity<Album>()
            .HasMany(a => a.Images)
            .WithMany(i => i.Albums);

        modelBuilder.Entity<Album>()
            .HasOne(a => a.CreatedBy)
            .WithMany()
            .HasForeignKey(a => a.CreatedById)
            .IsRequired();

        ConfigureBaseEntities(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    private void ConfigureBaseEntities(ModelBuilder modelBuilder)
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
    }

    private string GetDbConnectionString()
    {
        string connectionString = "";

        var host = configuration.Host ?? throw ConfigError("Database host", "Database.Host", "DB_HOST");
        var port = configuration.Port ?? "5432";
        var username = configuration.Username ?? throw ConfigError("Database user", "Database.Username", "DB_USER");
        var password = configuration.Password;
        var database = configuration.Database;

        connectionString += $"Host={host};Username={username}";
        connectionString += $";Port={port}";

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
