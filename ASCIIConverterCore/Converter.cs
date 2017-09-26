using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace ASCIIConverterCore
{
    public class Converter
    {
        /// <summary>
        /// Convert image to text of ASCII art
        /// </summary>
        /// <param name="SourceImage">Source image</param>
        /// <param name="Palette">Char palette</param>
        /// <param name="Height">Desirable height of text art</param>
        /// <param name="Width">Desirable width of text art</param>
        /// <returns>Text of ASCII art</returns>
        public static string[] GetText(Bitmap SourceImage, BasePalette Palette, int Height, int Width)
        {
            if (SourceImage.Height != Height || SourceImage.Width != Width)
            {
                SourceImage = ResizeImage(SourceImage, Width, Height);
            }

            String[] Text = new String[SourceImage.Height];
            for (int i = 0; i < SourceImage.Height; i++)
            {
                for (int j = 0; j < SourceImage.Width; j++)
                {
                    Text[i] += Palette.GetChar(SourceImage.GetPixel(j, i));
                }
            }
            return Text;
        }

        /// <summary>
        /// Convert image to text of ASCII art
        /// </summary>
        /// <param name="FileName">Name of image file</param>
        /// <param name="Palette">Char palette</param>
        /// <returns>Text of ASCII art</returns>
        /// <param name="Height">Desirable height of text art</param>
        /// <param name="Width">Desirable width of text art</param>
        public static string[] GetText(string FileName, BasePalette Palette, int Height, int Width)
        {
            Bitmap SourceImage;

            try
            {
                SourceImage = new Bitmap(FileName);
            }
            catch (Exception e)
            {
                throw e;
            }

            return GetText(SourceImage, Palette, Height, Width);
        }

        /// <summary>
        /// Convert image to text of ASCII art
        /// </summary>
        /// <param name="FileName">Name of image file</param>
        /// <param name="Palette">Char palette</param>
        /// <returns>Text of ASCII art</returns>
        public static string[] GetText(string FileName, BasePalette Palette)
        {
            Bitmap SourceImage;

            try
            {
                SourceImage = new Bitmap(FileName);
            }
            catch (Exception e)
            {
                throw e;
            }

            return GetText(SourceImage, Palette, SourceImage.Height, SourceImage.Width);
        }

        /// <summary>
        /// Convert image to text of ASCII art
        /// </summary>
        /// <param name="SourceImage">Source image</param>
        /// <param name="Palette">Char palette</param>
        /// <param name="Height">Height of image</param>
        /// <param name="Width">Width of image</param>
        public static string[] GetText(Bitmap SourceImage, BasePalette Palette)
        {
            return GetText(SourceImage, Palette, SourceImage.Height, SourceImage.Width);
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="Width">The width to resize to.</param>
        /// <param name="Height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int Width, int Height)
        {
            var destRect = new Rectangle(0, 0, Width, Height);
            var destImage = new Bitmap(Width, Height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
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
            }

            return destImage;
        }
    }
}
