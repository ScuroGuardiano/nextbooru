namespace Nextbooru.Core.Dto;

public class VoteChange
{
    public int Score { get; set; }
    
    // -1 -> Downvoted, 1 -> Upvoted, 0 -> Unvoted
    public int VoteScore { get; set; }

    public const int Upvote = 1;
    public const int Downvote = -1;
    public const int NoVote = 0;
}
