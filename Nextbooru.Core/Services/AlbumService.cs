using Nextbooru.Core.Dto.Requests;
using Nextbooru.Core.Models;

namespace Nextbooru.Core.Services;

public class AlbumService
{
    private readonly AppDbContext dbContext;

    public AlbumService(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Album> CreateAlbum(Guid userId, CreateAlbumRequest request)
    {
        var album = new Album()
        {
            Name = request.Name,
            Description = request.Description,
            IsPublic = request.IsPublic,
            CreatedById = userId
        };

        if (album.IsPublic)
        {
            album.PublishedAt = DateTime.UtcNow;
        }

        dbContext.Add(album);
        await dbContext.SaveChangesAsync();
        return album;
    }
}
