using Nextbooru.Core.Dto;
using Nextbooru.Core.Models;

namespace Nextbooru.Core.Services;

public class TagsService
{
    private readonly AppDbContext dbContext;
    private const int AutocompleteLimit = 10;
    
    public TagsService(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<TagDto>> Autocomplete(string phrase)
    {
        phrase = phrase.ToLower();
        var tags = await dbContext.Tags
            .Where(t => t.Name.StartsWith(phrase))
            .OrderByDescending(t => t.ImagesCount)
            .ThenBy(t => t.Name)
            .Take(AutocompleteLimit)
            .Select(t => TagDto.FromTagModel(t))
            .ToListAsync();
        return tags;
    }
}
