using System;
using System.IO;

namespace RayTracer;


public class Program
{
    
   public static void Main()
    {
        HittableList world = new HittableList();
        world.Add(new Sphere(new Point3(0, 0, -1), 0.5));
        world.Add(new Sphere(new Point3(0, -100.5, -1), 100));

        Camera cam = new Camera();

        cam.AspectRatio = 16.0 / 9.0;
        cam.ImageWidth = 400;

        cam.Render(world);
    }
    
}