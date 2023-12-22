using Nextbooru.Core.Models;

namespace Nextbooru.Core.Dto.Responses;

public class VoteChange
{
    public int Score { get; set; }
    
    public VoteScore VoteScore { get; set; }
    
}
