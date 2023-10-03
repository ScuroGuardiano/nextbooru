using Nextbooru.Core.Models;

namespace Nextbooru.Core.Services;

public class ImageService
{
    private readonly HornyContext dbContext;

    public ImageService(HornyContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task<Image> AddImage()
    {
        throw new NotImplementedException();
    }

    public async Task<List<Image>> ListRecentlyAddedImagesAsync()
    {
        return await dbContext.Images
            .Where(x => x.IsPublic)
            .OrderBy(x => x.CreatedAt)
            .Take(20)
            .ToListAsync();
    }
}
