using Microsoft.AspNetCore.Http;

namespace Nextbooru.Shared.Storage;

public interface IMediaStore
{
    /// <summary>
    /// Saves file in store and returns identifier needed to retrieve that file later.
    /// </summary>
    /// <param name="fileStream">File stream with data to save</param>
    /// <param name="fileExtension">File extension including the '.'</param>
    /// <returns>Identifier for future image retriaval</returns>
    Task<string> SaveFileAsync(Stream fileStream, string fileExtension);
    
    /// <summary>
    /// Retrieves file from store returning Stream.
    /// </summary>
    /// <param name="id">File identifier returned by method <see cref="SaveFileAsync"/></param>
    /// <returns>Stream with file content</returns>
    Task<Stream> GetFileStreamAsync(string id);
}
