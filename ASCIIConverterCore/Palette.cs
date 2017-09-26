using System;
using System.Drawing;

namespace ASCIIConverterCore
{
    public abstract class BasePalette
    {
        /// <summary>
        /// Calculate char code for predeterminded color
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>Char code</returns>
        public abstract char GetChar(Color color);
        
        /// <summary>
        /// Calculate luminance of color
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>Luminanve of color</returns>
        protected float GetLuminance(Color color)
        {
            return Math.Min((1 - color.A / 255f) + (color.R + color.G + color.B) / (255 * 3f), 1f);
            //return (0.299f * color.R / 255f + 0.587f * color.G / 255f + 0.114f * color.B / 255f); 
        }
    }

    public class Palette : BasePalette
    {
        public static string[] DefaultPatterns = {
            " █",
            " -;A",
            " .:-=+*#%@",
            " .'`^\",:; Il!i><~+-?]}1)|\\/trxnuczfjXYUJCLQ0OZmwqpdbkhao*#MW&8%B@$" }; 
        string Pattern;

        public Palette(string Pattern = " .:-=+*#%@")
        {
            this.Pattern = Pattern;
        }
        /// <summary>
        /// Calculate char code for determinded color
        /// </summary>
        /// <param name="color">Color</param>
        /// <returns>Char code</returns>
        public override char GetChar(Color color)
        {
            float l = 1f - GetLuminance(color);
            return Pattern[(int)(Math.Round(l * (Pattern.Length - 1)))];
        }
    }
}
