using System.Runtime.Serialization;
using FluentValidation;

namespace Nextbooru.Core.Dto.Requests;

public class ListImagesQuery
{
    /// <summary>
    /// Page result
    /// </summary>
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// How many results to put on page.<br/>
    /// Nextbooru can be configured to ignore values higher than certain threshold
    /// </summary>
    public int ResultsOnPage { get; set; }
    
    /// <summary>
    /// Tags are passed in a string with each tag separated by a comma.
    /// </summary>
    public string? Tags { get; set; }
    
    // TODO: Throw this away after implementing search parser.
    [IgnoreDataMember]
    public string[] TagsArray => string.IsNullOrEmpty(Tags) ? Array.Empty<string>() : Tags.Split(Comma);

    private const string Comma = ",";

    public class ListImagesQueryValidator : AbstractValidator<ListImagesQuery>
    {
        public ListImagesQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.ResultsOnPage).GreaterThanOrEqualTo(0);
        }
    }
}
