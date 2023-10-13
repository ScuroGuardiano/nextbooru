using Microsoft.OpenApi.Extensions;
using Nextbooru.Core.Models;

namespace Nextbooru.Core.Dto;

public class TagDto
{
    public required string Name { get; set; }
    public required string TagType { get; set; }

    public static TagDto FromTagModel(Tag tag)
    {
        return new TagDto()
        {
            Name = tag.Name,
            TagType = tag.TagType.GetDisplayName()
        };
    }
}
