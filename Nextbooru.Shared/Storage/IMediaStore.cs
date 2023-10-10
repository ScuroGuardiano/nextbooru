using Microsoft.AspNetCore.Http;

namespace Nextbooru.Shared.Storage;

public interface IMediaStore
{
    Task<string> SaveFileAsync(IFormFile file);
    Stream GetFileStreamAsync(string id);
}
