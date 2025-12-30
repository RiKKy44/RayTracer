using System;
using System.IO;

namespace RayTracer;


public class Program
{
    
    private static Color3 RayColor(Ray ray, IHittable world)
    {
        HitRecord rec;
        if (world.Hit(ray, new Interval(0,Utils.Infinity), out rec))
        {
            return 0.5 * (rec.Normal + new Color3(1, 1, 1));
        }
        Vec3 unitDirection = Vec3.UnitVector(ray.Direction);
        var a = 0.5 * (unitDirection.Y + 1.0);
        return (1.0 - a) * new Color3(1.0, 1.0, 1.0) + a * new Color3(0.5, 0.7, 1.0);
    }
    public static void Main()
    { 
        //image
        const double aspectRatio = 16.0 / 9.0;
        const int imageWidth = 400;
        // Calculate the image height, and ensure that it's at least 1.
        var imageHeight = (int)(imageWidth / aspectRatio);
        imageHeight = (imageHeight < 1) ? 1 : imageHeight;
        //world

        HittableList world = new HittableList();
        world.Add(new Sphere(new Point3(0, 0, -1), 0.5));
        world.Add(new Sphere(new Point3(0, -100.5, -1), 100));
        //Camera
        var focalLength = 1.0;
        var viewpointHeight = 2.0;
        var viewpointWidth = viewpointHeight*(double)imageWidth/imageHeight;
        Point3 cameraCenter = new Point3(0, 0, 0);
        // Calculate the vectors across the horizontal and down the vertical viewport edges.
        //U - x axis on viewpoint, V - y axis, it increases when going down
        var viewpointU = new Vec3(viewpointWidth, 0, 0);
        var viewpointV = new Vec3(0,-viewpointHeight, 0);
        // Calculate the horizontal and vertical delta vectors from pixel to pixel
        //this is distance between centers of the pixels
        var pixelDeltaU = viewpointU/imageWidth;
        var pixelDeltaV = viewpointV/imageHeight;
        
        // Calculate the location of the upper left pixel.
        //we subtract viewpointV even though we go up because in our viewpoint Y-axis is inverted
        var viewpointUpperLeft = cameraCenter - new Vec3(0,0,focalLength) - viewpointU/2 - viewpointV/2;
        var pixel00 = viewpointUpperLeft + 0.5*(pixelDeltaU+pixelDeltaV);
        
        //render
        using (StreamWriter writer = new StreamWriter("image.ppm"))
        {
            writer.WriteLine("P3");
            writer.WriteLine($"{imageWidth} {imageHeight}");
            writer.WriteLine("255");

            for (int i = 0; i < imageHeight; i++)
            {
                Console.Error.Write($"\rScanlines remaining: {imageHeight - i} ");
                for (int j = 0; j < imageWidth; j++)
                {
                    var pixelCenter = pixel00 + j * pixelDeltaU + i * pixelDeltaV;
                    var rayDirection = pixelCenter - cameraCenter;
                    Ray ray = new Ray(cameraCenter, rayDirection);
                    Color3 pixelColor = RayColor(ray,world);
                    
                    Color.WriteColor(writer,pixelColor);
                }
            }

            Console.Error.Write("\nDone");
        }
    }
    
}