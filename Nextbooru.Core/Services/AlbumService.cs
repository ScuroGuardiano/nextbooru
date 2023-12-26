using Nextbooru.Auth.Models;
using Nextbooru.Core.Dto.Requests;
using Nextbooru.Core.Dto.Responses;
using Nextbooru.Core.Models;
using System.Linq.Dynamic.Core;

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

    public async Task<Album?> GetAlbumAsync(long id)
    {
        return await dbContext.Albums.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<ListResponse<AlbumDto>> ListAlbumsAsync(ListAlbumsQuery queryData, User? user)
    {
        var query = dbContext.Albums.AsQueryable();

        if (user is null)
        {
            query = query.Where(a => a.IsPublic);
        }
        else if (!user.IsAdmin)
        {
            query = query.Where(a => a.IsPublic || a.CreatedById == user.Id);
        }

        if (queryData.Name is not null)
        {
            query = query.Where(a => a.Name.StartsWith(queryData.Name));
        }

        if (queryData.OrderBy is not null)
        {
            var orderable = query.OrderBy($"{queryData.OrderBy} {queryData.OrderDirection}");
            if (queryData.OrderBy != "Id")
            {
                orderable = orderable.ThenBy(a => a.Id);
            }

            query = orderable;
        }
        else
        {
            query = query.OrderBy(a => a.Id);
        }

        var total = await query.CountAsync();
        var totalPages = (int)Math.Ceiling((decimal)total / queryData.ResultsOnPage);
        var albums = await query
            .Skip(queryData.ResultsOnPage * (queryData.Page - 1))
            .Take(queryData.ResultsOnPage)
            .ToListAsync();

        return new ListResponse<AlbumDto>()
        {
            Data = albums.Select(AlbumDto.From).ToList(),
            Page = queryData.Page,
            RecordsPerPage = queryData.ResultsOnPage,
            LastRecordId = albums.LastOrDefault()?.Id ?? 0,
            TotalPages = totalPages,
            TotalRecords = total
        };
    }
}

