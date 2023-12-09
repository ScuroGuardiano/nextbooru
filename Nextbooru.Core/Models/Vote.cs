using Nextbooru.Auth.Models;
using Nextbooru.Shared;

namespace Nextbooru.Core.Models;

public class Vote : BaseEntity
{
    public long Id { get; set; }
    
    public Guid UserId { get; set; }

    public User? User { get; set; }
    
    /// <summary>
    ///  1: Upvote
    /// -1: Downvote
    /// </summary>
    [AllowedValues([1, -1])]
    public int VoteScore { get; set; }
    
    public static readonly int Upvote = 1;
    public static readonly int Downvote = -1;
}
