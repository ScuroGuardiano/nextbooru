using Microsoft.Extensions.Options;
using Nextbooru.Core.Exceptions;
using Nextbooru.Shared.Storage;

namespace Nextbooru.Core.Services;

public class LocalMediaStore : IMediaStore
{
    private readonly AppSettings configuration;

    public LocalMediaStore(IOptions<AppSettings> options)
    {
        configuration = options.Value;
    }
    
    public async Task<string> SaveFileAsync(Stream fileStream, string fileExtension)
    {
        var randomName = Path.GetRandomFileName()
            .Replace(".", "")
                         + fileExtension;

        if (!Directory.Exists(configuration.MediaStoragePath))
        {
            Directory.CreateDirectory(configuration.MediaStoragePath);
        }
        
        await using var file = File.Open(Path.Join(configuration.MediaStoragePath, randomName), FileMode.CreateNew);
        await fileStream.CopyToAsync(file);
        
        return randomName;
    }

    public Task<Stream> GetFileStreamAsync(string id)
    {
        try
        {
            var file = File.OpenRead(Path.Join(configuration.MediaStoragePath, id));
            return Task.FromResult((Stream)file);
        }
        catch (System.IO.FileNotFoundException)
        {
            throw new FileDoesNotExistsException(id);
        }
    }
}
