using Microsoft.OpenApi.Extensions;
using Nextbooru.Core.Models;

namespace Nextbooru.Core.Dto.Responses;

public class TagDto
{
    public required string Name { get; set; }
    public required string TagType { get; set; }
    
    public int Count { get; set; }

    public static TagDto FromTagModel(Tag tag)
    {
        return new TagDto()
        {
            Name = tag.Name,
            TagType = tag.TagType.GetDisplayName().ToLower(),
            Count = tag.ImagesCount
        };
    }
}
