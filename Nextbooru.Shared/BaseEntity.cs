using Nextbooru.Shared.QueryHelpers;

namespace Nextbooru.Shared;

public class BaseEntity
{
    [Orderable]
    public DateTime CreatedAt { get; set; }

    [Orderable]
    public DateTime UpdatedAt { get; set; }
}
