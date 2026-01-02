using System;

namespace RayTracer;

public class Program
{
    public static void Main()
    {
        HittableList world = new HittableList();

        var materialGround = new Lambertian(new Vec3(0.5, 0.5, 0.5));
        world.Add(new Sphere(new Point3(0, -1000, 0), 1000, materialGround));

        for (int a = -11; a < 11; a++)
        {
            for (int b = -11; b < 11; b++)
            {
                var chooseMat = Utils.RandomDouble();
                Point3 center = new Point3(a + 0.9 * Utils.RandomDouble(), 0.2, b + 0.9 * Utils.RandomDouble());

                if ((center - new Point3(4, 0.2, 0)).Length() > 0.9)
                {
                    IMaterial sphereMaterial;

                    if (chooseMat < 0.8)
                    {
                        var albedo = Vec3.Random() * Vec3.Random();
                        sphereMaterial = new Lambertian(albedo);
                        world.Add(new Sphere(center, 0.2, sphereMaterial));
                    }
                    else if (chooseMat < 0.95)
                    {
                        var albedo = Vec3.Random(0.5, 1);
                        var fuzz = Utils.RandomDouble(0, 0.5);
                        sphereMaterial = new Metal(albedo, fuzz);
                        world.Add(new Sphere(center, 0.2, sphereMaterial));
                    }
                    else
                    {
                        sphereMaterial = new Dielectric(1.5);
                        world.Add(new Sphere(center, 0.2, sphereMaterial));
                    }
                }
            }
        }

        var material1 = new Dielectric(1.5);
        world.Add(new Sphere(new Point3(0, 1, 0), 1.0, material1));

        var material2 = new Lambertian(new Vec3(0.4, 0.2, 0.1));
        world.Add(new Sphere(new Point3(-4, 1, 0), 1.0, material2));

        var material3 = new Metal(new Vec3(0.7, 0.6, 0.5), 0.0);
        world.Add(new Sphere(new Point3(4, 1, 0), 1.0, material3));

        Camera cam = new Camera();

        cam.AspectRatio = 16.0 / 9.0;
        cam.ImageWidth = 1200;
        cam.SamplesPerPixel = 500;
        cam.MaxDepth = 50;

        cam.Vfov = 20;
        cam.LookFrom = new Point3(13, 2, 3);
        cam.LookAt = new Point3(0, 0, 0);
        cam.VUp = new Vec3(0, 1, 0);

        cam.DefocusAngle = 0.6;
        cam.FocusDist = 10.0;

        cam.Render(world);
    }
}