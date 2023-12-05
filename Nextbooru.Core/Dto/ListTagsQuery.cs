using FluentValidation;
using Microsoft.Extensions.Options;
using Nextbooru.Core.Models;
using Nextbooru.Shared.QueryHelpers;

namespace Nextbooru.Core.Dto;

public class ListTagsQuery
{
    /// <summary>
    /// Results page
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// How many results to put on page.<br/>
    /// Nextbooru can be configured to ignore values higher than certain threshold
    /// </summary>
    public int ResultsOnPage { get; set; }
    
    public string? Name { get; set; }
    public string? SortOrder { get; set; } = "asc";
    public string? OrderBy { get; set; } = "Id";

    public class ListTagsQueryValidator : AbstractValidator<ListTagsQuery>
    {
        public ListTagsQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            
            // Don't add any correntness checks except >= 0 here (like using configuration.MaxResultsPerPage)
            // As I want incorrect values to be ingored and default or max used instead.
            RuleFor(x => x.ResultsOnPage).GreaterThanOrEqualTo(0);

            RuleFor(x => x.SortOrder)
                .Must(LowercaseBeOneOf(["asc", "desc"]))
                .WithMessage("{PropertyName} can be only 'asc' or 'desc', your value is: {PropertyValue}");
            
            RuleFor(x => x.OrderBy)
                .Must(BeOneOf(QueryHelper.GetOrderableFields<Tag>()))
                .WithMessage("You can't order Tags by {PropertyValue}. Endpoint <api_prefix>/tags/orderable-fields will return available fields to you <3");
        }

        private Func<string?, bool> LowercaseBeOneOf(string[] values)
        {
            return (string? x) => x is null || values.Contains(x.ToLower());
        }
        
        private Func<string?, bool> BeOneOf(IEnumerable<string> values)
        {
            return (string? x) => x is null || values.Contains(x);
        }
    }
}
