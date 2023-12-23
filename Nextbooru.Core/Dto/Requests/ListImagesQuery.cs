using System.Runtime.Serialization;
using FluentValidation;

namespace Nextbooru.Core.Dto.Requests;

public class ListImagesQuery : PaginatedListQuery
{
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
            Include(new PaginatedListQueryValidator());
        }
    }
}
