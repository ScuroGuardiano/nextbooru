using System.Data;
using Nextbooru.Core.Dto;
using Nextbooru.Core.Exceptions;
using Nextbooru.Core.Models;

namespace Nextbooru.Core.Services;

public class ImageVotingService
{
    private readonly AppDbContext dbContext;
    
    public ImageVotingService(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// Upvotes an image as provided user.
    /// If the image has been already upvoted by the user then this method
    /// will remove user's vote. If the image has been already downvoted by the user
    /// then downvote will be changed to upvote
    /// </summary>
    /// <returns>VoteChange with score after an operation</returns>
    public Task<VoteChange> SwitchUpvote(Guid userId, long imageId)
    {
        return SwitchVote(userId, imageId, VoteScore.Upvote);
    }

    /// <summary>
    /// Downvotes an image as provided user.
    /// If the image has been already downvoted by the user then this method
    /// will remove user's vote. If the image has been already upvoted by the user
    /// then upvote will be changed to downvote.
    /// </summary>
    /// <returns>VoteChange with score after an operation</returns>
    public Task<VoteChange> SwitchDownvote(Guid userId, long imageId)
    {
        return SwitchVote(userId, imageId, VoteScore.Downvote);
    }

    private async Task<VoteChange> SwitchVote(Guid userId, long imageId, VoteScore voteValue)
    {
        // RepeatableRead is enough here, because
        // 1. I update row in Images table, so two concurrent inserts to ImageVotes shouldn't be possible
        // 2. There's an unique index on userId and imageId on ImageVotes, so inserting two ImageVotes is impossible.
        await using var transaction = await dbContext.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead);

        try
        {
            var vote = await dbContext.ImageVotes
                .FirstOrDefaultAsync(iv => iv.ImageId == imageId && iv.UserId == userId);
            int currentScore;
            int voteChangeVoteScore = VoteChange.NoVote;
            
            if (vote is not null)
            {
                if (vote.VoteScore != voteValue)
                {
                    var previousScore = vote.VoteScore;
                    vote.VoteScore = voteValue;
                    // Cancel previous vote and apply new vote
                    currentScore = await ChangeImageScore(imageId, (-1 * (int)previousScore) + (int)voteValue);
                    voteChangeVoteScore = (int)voteValue;
                }
                else
                {
                    dbContext.Remove(vote);
                    currentScore = await ChangeImageScore(imageId, -1 * (int)voteValue);
                }

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                
                return new VoteChange()
                {
                    Score = currentScore,
                    VoteScore = voteChangeVoteScore
                };
            }

            vote = new ImageVote()
            {
                UserId = userId,
                VoteScore = voteValue,
                ImageId = imageId
            };
            
            dbContext.Add(vote);
            currentScore = await ChangeImageScore(imageId, (int)voteValue);
            await dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return new VoteChange()
            {
                Score = currentScore,
                VoteScore = (int)voteValue
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    private async Task<int> ChangeImageScore(long imageId, int change)
    {
        var imageScore = await dbContext.Images
            .Where(i => i.Id == imageId)
            .Select(i => new { i.Score })
            .FirstOrDefaultAsync();

        if (imageScore is null)
        {
            throw new NotFoundException(imageId);
        }

        var partialImage = new Image(imageId)
        {
            Score = imageScore.Score + change
        };
        
        dbContext.Attach(partialImage);
        dbContext.Entry(partialImage).Property(i => i.Score).IsModified = true;
        
        return partialImage.Score;
    }
}
