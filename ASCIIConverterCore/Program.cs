using System;
using System.IO;

namespace ASCIIConverterCore
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "--help")
            {
                PrintHelp();
                return;
            }

            string FileName = args[0];
            int Height = 0;
            int Width = 0;

            bool UseUserPalette = args.Length == 5;
            bool AllowableCountOfArgs = args.Length == 5 || args.Length == 4;
            bool UseResize = AllowableCountOfArgs && int.TryParse(args[2], out Height) && int.TryParse(args[3], out Width); 

            if (!UseResize)
            {
                Console.WriteLine("[Error] Invalid Arguments. Use --help for more details.");
                return;
            }

            string[] text = null;

            try
            {
                text = UseUserPalette ?
                    Converter.GetText(FileName, new Palette(' ' + args[4].Trim('\'')), Height, Width) :
                    Converter.GetText(FileName, new Palette(), Height, Width);
            }
            catch
            {
                Console.WriteLine("[Error]: Couldn't open image file.");
                return;
            }

            try
            {
                using (StreamWriter sw = File.CreateText(args[1]))
                {
                    foreach (string line in text)
                    {
                        sw.WriteLine(line);
                    }
                }
            }
            catch
            {
                Console.WriteLine("[Error]: Couldn't write output text file.");
                return;
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Simple image to ASCII art converter.");
            Console.WriteLine("Open image file and save ascii art to output file");
            Console.WriteLine("Supported image formats: BMP, GIF, EXIF, JPG, PNG and TIFF");
            Console.WriteLine("------------------------ USAGE: --------------------------");
            Console.WriteLine("Parameters: [image-file-name] [output-file] [height] [width] '[palette]'");
            Console.WriteLine("Height and Width describe size of output ASCII art. Palette is optional and describse char palette for ASCII art. If palette don't described, program will use default palette. IMPORTANT: Palette have to be surrounded by quotes.");
            Console.WriteLine("Example: ASCIIConverterCore.exe puppies.jpg art.txt 100 100 '.:/#@'");
            Console.WriteLine("Example: ASCIIConverterCore.exe puppies.jpg art.txt 100 100");
        }
    }
}