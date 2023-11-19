namespace Nextbooru.Core.Dto;

public class GetImageFileQuery
{
    public int W { get; set; } = 0;

    public string Mode { get; set; } = AppConstants.ImageModes.Raw;
}