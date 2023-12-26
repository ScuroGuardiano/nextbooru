using Nextbooru.Auth.Models;
using Nextbooru.Shared;

namespace Nextbooru.Core.Models;

public class Vote : BaseEntity
{
    public long Id { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }

    public VoteScore VoteScore { get; set; }
}
