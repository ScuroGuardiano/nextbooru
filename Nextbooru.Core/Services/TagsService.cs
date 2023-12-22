using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using Nextbooru.Core.Dto;
using Nextbooru.Core.Dto.Requests;
using Nextbooru.Core.Dto.Responses;
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

    public async Task<ListResponse<TagDto>> ListTags(ListTagsQuery queryData)
    {
        ArgumentNullException.ThrowIfNull(queryData.ResultsOnPage);

        var query = dbContext.Tags.AsQueryable();

        if (queryData.Name is not null)
        {
            query = query.Where(t => t.Name == queryData.Name);
        }
            
        if (queryData.OrderBy is not null)
        {
            var orderable = query.OrderBy($"{queryData.OrderBy} {queryData.OrderDirection}");
            if (queryData.OrderBy != "Id")
            {
                orderable = orderable.ThenBy(t => t.Id);
            }

            query = orderable;
        }
        else
        {
            query = query.OrderBy(t => t.Id);
        }

        var total = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((decimal)total / queryData.ResultsOnPage);
        var tags = await query
            .Skip(queryData.ResultsOnPage * (queryData.Page - 1))
            .Take(queryData.ResultsOnPage)
            .ToListAsync();
        
        return new ListResponse<TagDto>()
        {
            Data = tags.Select(TagDto.FromTagModel).ToList(),
            Page = queryData.Page,
            RecordsPerPage = queryData.ResultsOnPage,
            LastRecordId = tags.LastOrDefault()?.Id ?? 0,
            TotalPages = totalPages,
            TotalRecords = total
        };
    }
}
