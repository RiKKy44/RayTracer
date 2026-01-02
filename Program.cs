using System;
using System.IO;
using System.Linq.Expressions;

namespace RayTracer;


public class Program
{
    
   public static void Main(string[] args)
    {
        //default values:
        int width = 400;
        int samples = 20;
        int depth = 10;
        string filename = "render.jpg";


        for (int i = 0; i < args.Length; i++)
        {
            try
            {
                switch(args[i]){
                    case "-w":
                        width = int.Parse(args[i + 1]);
                        i++;
                        break;
                    case "-s":
                        samples = int.Parse(args[i + 1]);
                        i++;
                        break;

                    case "-d":
                        depth = int.Parse(args[i + 1]);
                        i++;
                        break;
                    case "-f":
                        filename = args[i + 1];
                        i++;
                        break;
                    }
            }
            catch (Exception)
            {
                Console.WriteLine($"Error while reading parameter {args[i]}. Program will use default values");
            }
        
        }
        Console.WriteLine($"Rendering: {width}px width, {samples} samples, {depth} depth --> {filename}");




        HittableList world = new HittableList();


        var materialGround = new Lambertian(new Color3(0.8, 0.8, 0.0));

        var materialCenter = new Lambertian(new Color3(0.1, 0.2, 0.5));

        var materialLeft = new Dielectric(1.50);

        var materialBubble = new Dielectric(1.00 / 1.50);

        var materialRight = new Metal(new Color3(0.8, 0.6, 0.2), 1.0);

        world.Add(new Sphere(new Point3(0.0, -100.5, -1.0), 100.0, materialGround));

        world.Add(new Sphere(new Point3(0.0, 0.0, -1.2), 0.5, materialCenter));

        world.Add(new Sphere(new Point3(-1.0, 0.0, -1.0), 0.5, materialLeft));

        world.Add(new Sphere(new Point3(-1.0, 0.0, -1.0), 0.4, materialBubble));

        world.Add(new Sphere(new Point3(1.0, 0.0, -1.0), 0.5, materialRight));

        Camera cam = new Camera();

        cam.AspectRatio = 16.0 / 9.0;

        cam.ImageWidth = 400;

        cam.SamplesPerPixel = 100;

        cam.MaxDepth = 50;

        cam.Vfov = 20;

        cam.LookFrom = new Point3(-2, 2, 1);
        cam.LookAt = new Point3(0, 0, -1);
        cam.VUp = new Vec3(0, 1, 0);
        cam.DefocusAngle = 10.0;
        cam.FocusDist = 3.4;
        cam.Render(world,filename);
    }
    
}