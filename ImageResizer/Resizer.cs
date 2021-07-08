using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace ImageResizer
{
    public class Resizer
    {
        public static ResizedImage ResizeToNewWidth(MemoryStream memoryStream, int originalWidth, int originalHeight,
            int newWidth)
        {
            var resizeMultiplier = newWidth / (decimal) originalWidth;
            var newHeight = (int) (originalHeight * resizeMultiplier);

            var destRect = new Rectangle(0, 0, newWidth, newHeight);

            memoryStream.Position = 0;

            using var image = Image.FromStream(memoryStream);
            using var destImage = new Bitmap(newWidth, newHeight);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using var graphics = Graphics.FromImage(destImage);
            graphics.CompositingMode = CompositingMode.SourceCopy;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            using (var wrapMode = new ImageAttributes())
            {
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            var output = new MemoryStream();
            destImage.Save(output, image.RawFormat);

            return new ResizedImage(destImage.Height, destImage.Width, output);
        }

        public static byte[] ConvertImageToByte(Image image)
        {
            using var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            return stream.ToArray();
        }
    }
}