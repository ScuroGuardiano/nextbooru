namespace Nextbooru.Core.Services;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Image = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Formats.Gif;

public class ImageConvertionOptions
{
    public int Width { get; set; }
    public int Quality { get; set; }
    public required String Format { get; set; }
}

public class ImageConvertionService
{
    public async Task<(int width, int height)> ConvertImageAsync(Stream input, Stream output, ImageConvertionOptions options)
    {
        using var image = await Image.LoadAsync(input);
        return await ConvertImageAsync(image, output, options);
    }

    public async Task<(int width, int height)> ConvertImageAsync(Image input, Stream output, ImageConvertionOptions options)
    {
        Image resultImage = input;

        try
        {
            if (options.Width != 0)
            {
                resultImage = input.Clone(x => x.Resize(options.Width, 0));
            }

            ImageEncoder encoder;

            switch (options.Format)
            {
                case "jpg":
                case "jpeg":
                    if (options.Quality > 0)
                    {
                        encoder = new JpegEncoder()
                        {
                            Quality = options.Quality
                        };
                    }
                    else
                    {
                        encoder = new JpegEncoder();
                    }
                    break;

                case "webp":
                    if (options.Quality > 0)
                    {
                        encoder = new WebpEncoder()
                        {
                            Quality = options.Quality
                        };
                    }
                    else
                    {
                        encoder = new WebpEncoder();
                    }
                    break;

                case "png":
                    encoder = new PngEncoder();
                    break;

                case "gif":
                    encoder = new GifEncoder();
                    break;

                default:
                    throw new FormatException($"Format {options.Format} is not supported.");
            }

            await resultImage.SaveAsync(output, encoder);
            return (resultImage.Width, resultImage.Height);
        }
        finally
        {
            if (resultImage != input)
            {
                resultImage.Dispose();
            }
        }

    }
}