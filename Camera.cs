using System;
namespace RayTracer;




public class Camera
{
    public double AspectRatio { get; set; } = 1.0;
    public double ImageWidth { get; set; } = 100;


    private int _imageHeight;
    private Point3 _center;
    private Point3 _pixel00Loc;
    private Vec3 _pixelDeltaU;
    private Vec3 _pixelDeltaV;

    public void Render(IHittable world)
    {
        this.Initialize();


        using(StreamWriter writer = new StreamWriter("image.ppm"))
        {
            writer.WriteLine("P3");
            writer.WriteLine($"{ImageWidth} {_imageHeight}");
            writer.WriteLine("255");
            

            for(int j = 0; j < _imageHeight; j++)
            {
                Console.Error.Write($"\rScanlines remaining: {_imageHeight - j} ");
                for(int i = 0; i < ImageWidth; i++)
                {
                    var pixelCenter = _pixel00Loc + (i * _pixelDeltaU) + (j * _pixelDeltaV);
                    var rayDirection = pixelCenter - _center;
                    Ray ray = new Ray(_center, rayDirection);

                    Vec3 pixelColor = RayColor(ray, world);

                    Color.WriteColor(writer, pixelColor);
                }


                Console.Error.Write("\nDone");
            }
        }
    }


    private void Initialize()
    {
        _imageHeight = (int)(ImageWidth / AspectRatio);
        _imageHeight = (_imageHeight<1)?1:_imageHeight;

        _center = new Point3(0, 0, 0);

        var focalLength = 1.0;
        var viewportHeight = 2.0;

        var viewportWidth = viewportHeight * (double)ImageWidth / _imageHeight;

        var viewportU = new Vec3(viewportWidth, 0, 0);
        var viewportV = new Vec3(0, -viewportHeight, 0);

        _pixelDeltaU = viewportU / ImageWidth;
        _pixelDeltaV = viewportV / _imageHeight;

        var viewportUpperLeft = _center - new Vec3(0, 0, focalLength) - viewportU / 2 - viewportV / 2;
        _pixel00Loc = viewportUpperLeft + 0.5 * (_pixelDeltaU + _pixelDeltaV);
    }

    private Vec3 RayColor(Ray ray, IHittable world)
    {
        HitRecord rec;
        if (world.Hit(ray, new Interval(0, Utils.Infinity), out rec))
        {
            return 0.5 * (rec.Normal + new Vec3(1, 1, 1));
        }

        Vec3 unitDirection = Vec3.UnitVector(ray.Direction);
        var a = 0.5 * (unitDirection.Y + 1.0);
        return (1.0 - a) * new Vec3(1.0, 1.0, 1.0) + a * new Vec3(0.5, 0.7, 1.0);
    }
}