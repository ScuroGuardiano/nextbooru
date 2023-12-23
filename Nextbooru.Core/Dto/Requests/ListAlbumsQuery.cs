using FluentValidation;
using Nextbooru.Core.Models;
using Nextbooru.Shared.QueryHelpers;

namespace Nextbooru.Core.Dto.Requests;

public class ListAlbumsQuery : PaginatedListQuery
{
 
    public string? Name { get; set; }
    public string? OrderDirection { get; set; } = "asc";
    public string? OrderBy { get; set; } = "Id";

    public class ListAlbumsQueryValidator : AbstractValidator<ListAlbumsQuery>
    {
        public ListAlbumsQueryValidator()
        {
            Include(new PaginatedListQueryValidator());
            
            RuleFor(x => x.OrderDirection)
                .Must(LowercaseBeOneOf(["asc", "desc"]))
                .WithMessage("{PropertyName} can be only 'asc' or 'desc', your value is: {PropertyValue}");
            
            RuleFor(x => x.OrderBy)
                .Must(BeOneOf(QueryHelper.GetOrderableFields<Album>()))
                .WithMessage("You can't order Albums by {PropertyValue}. Endpoint <api_prefix>/albums/orderable-fields will return available fields to you <3");
        }
        
        private Func<string?, bool> LowercaseBeOneOf(string[] values)
        {
            return x => x is null || values.Contains(x.ToLower());
        }
        
        private Func<string?, bool> BeOneOf(IEnumerable<string> values)
        {
            return x => x is null || values.Contains(x);
        }
    }
}
