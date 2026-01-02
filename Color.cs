using System.IO;
namespace RayTracer;


public static class Color
{
    public static void WriteColor(StreamWriter writer, Vec3 color, int samplesPerPixel)
    {
        double r = color.X / samplesPerPixel;
        double g = color.Y / samplesPerPixel;
        double b = color.Z / samplesPerPixel;


        Interval intensity = new Interval(0.000, 0.999); 
        int rbyte = (int)(256 * intensity.Clamp(r));
        int gbyte = (int)(256 * intensity.Clamp(g));
        int bbyte = (int)(256 * intensity.Clamp(b));
        
        writer.WriteLine($"{rbyte} {gbyte} {bbyte}");
        
    }
} 

