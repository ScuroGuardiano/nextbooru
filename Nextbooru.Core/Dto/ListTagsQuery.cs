namespace Nextbooru.Core.Dto;

public class ListTagsQuery
{
    /// <summary>
    /// Results page
    /// </summary>
    [Range(1, int.MaxValue)]
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// How many results to put on page.<br/>
    /// Nextbooru can be configured to ignore values higher than certain threshold
    /// </summary>
    [Range(1, int.MaxValue)]
    public int? ResultsOnPage { get; set; }
    
    public string? Name { get; set; }
    
    // TODO: Migrate finally to fluent validation and make validation here
    public string? SortOrder { get; set; } = "asc";

    // TODO: Add fluent validation to this aswell.
    public string? OrderBy { get; set; } = "Id";
}
