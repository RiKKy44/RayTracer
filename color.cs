using System.IO;
namespace RayTracer;


public static class Color
{
    public static void WriteColor(StreamWriter writer, Vec3 color)
    {
        double r = color.X;
        double g = color.Y;
        double b = color.Z;
        
        int rbyte = (int)(r*255.999);
        int gbyte = (int)(g*255.999);
        int bbyte = (int)(b*255.999);
        
        writer.WriteLine($"{rbyte} {gbyte} {bbyte}");
        
    }
} 

