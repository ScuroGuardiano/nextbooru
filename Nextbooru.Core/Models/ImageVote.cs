namespace Nextbooru.Core.Models;

public class ImageVote : Vote
{
    public long ImageId { get; set; }
    public Image? Image { get; set; }
}
