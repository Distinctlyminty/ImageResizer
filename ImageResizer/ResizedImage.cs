using System.IO;

namespace ImageResizer
{
    public class ResizedImage
    {
        public ResizedImage(int height, int width, Stream stream)
        {
            Height = height;
            Width = width;
            Stream = stream;
        }

        public int Height { get; }
        public int Width { get; }
        public Stream Stream { get; }
    }
}