using FluentValidation;

namespace Nextbooru.Core.Dto.Requests;

public class PaginatedListQuery
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

    protected class PaginatedListQueryValidator : AbstractValidator<PaginatedListQuery>
    {
        public PaginatedListQueryValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            
            // Don't add any correntness checks except >= 0 here (like using configuration.MaxResultsPerPage)
            // As I want incorrect values to be ingored and default or max used instead.
            RuleFor(x => x.ResultsOnPage).GreaterThanOrEqualTo(0);
        }
    }
}
