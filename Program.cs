using System;
using System.IO;

namespace RayTracer;


public class Program
{
    
   public static void Main()
    {
        HittableList world = new HittableList();


        var materialGround = new Lambertian(new Color3(0.8, 0.8, 0.0));

        var materialCenter = new Lambertian(new Color3(0.1, 0.2, 0.5));

        var materialLeft = new Metal(new Color3(0.8, 0.8, 0.8),0.3);

        var materialRight = new Metal(new Color3(0.8, 0.6, 0.2),1.0);

        world.Add(new Sphere(new Point3(0.0, -100.5, -1.0), 100.0, materialGround));

        world.Add(new Sphere(new Point3(0.0, 0.0, -1.2), 0.5, materialCenter));

        world.Add(new Sphere(new Point3(-1.0, 0.0, -1.0), 0.5, materialLeft));

        world.Add(new Sphere(new Point3(1.0, 0.0, -1.0), 0.5, materialRight));






        Camera cam = new Camera();

        cam.AspectRatio = 16.0 / 9.0;
        cam.ImageWidth = 400;
        cam.SamplesPerPixel = 100;
        cam.MaxDepth = 50;
        cam.Render(world);
    }
    
}