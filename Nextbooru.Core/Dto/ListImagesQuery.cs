using System.Runtime.Serialization;

namespace Nextbooru.Core.Dto;

public class ListImagesQuery
{
    /// <summary>
    /// Page result
    /// </summary>
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// How many results to put on page.<br/>
    /// Nextbooru can be configured to ignore values higher than certain threshold
    /// </summary>
    [Range(1, int.MaxValue)]
    public int? ResultsOnPage { get; set; }
    
    /// <summary>
    /// Tags are passed in a string with each tag separated by a comma.
    /// </summary>
    public string? Tags { get; set; }
    
    [IgnoreDataMember]
    public string[] TagsArray => string.IsNullOrEmpty(Tags) ? Array.Empty<string>() : Tags.Split(Comma);

    private const string Comma = ",";
}
