using Microsoft.DeepZoomTools;
using System;
using System.IO;

namespace Etch.OrchardCore.LeafletTileGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists(args[1]))
            {
                Console.WriteLine($"Unable to find image file at {args[1]}");
                return;
            }

            if (!Directory.Exists(args[0]))
            {
                Directory.CreateDirectory(args[0]);
            }

            ImageCreator imageCreator = new ImageCreator();
            imageCreator.Create(args[1], args[0]);
        }
    }
}
