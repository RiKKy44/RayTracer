using System;
using System.IO;

namespace RayTracer;


class Program
{
    static void Main(string[] args)
    {
        const int width = 256;
        const int height = 256;

        using (StreamWriter writer = new StreamWriter("image.ppm"))
        {
            writer.WriteLine("P3");
            writer.WriteLine($"{width} {height}");
            writer.WriteLine("255");

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    var r = (double)i / (width - 1);
                    var g = (double)j / (height - 1);
                    var b = 0.0;

                    int ir = (int)(255.999 * r);
                    int ig = (int)(255.999 * g);
                    int bl = (int)(255.999 * b);
                    
                    writer.WriteLine($"{ir} {ig} {bl}");
                }
            }
        }
    }
    
}