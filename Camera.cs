using System;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
namespace RayTracer;




public class Camera
{
    public double AspectRatio { get; set; } = 1.0;

    public double ImageWidth { get; set; } = 100;

    public int SamplesPerPixel { get; set; } = 10;

    public int MaxDepth { get; set; } = 10;

    public double Vfov { get; set; } = 90;

    public Point3 LookFrom { get; set; } = new Point3(0, 0, -1);

    public Point3 LookAt { get; set; } = new Point3(0, 0, 0);

    public Vec3 VUp { get; set; } = new Vec3(0, 1, 0);

    public double DefocusAngle { get; set; } = 0;

    public double FocusDist { get; set; } = 10;

    private int _imageHeight;

    private Point3 _center;

    private Point3 _pixel00Loc;

    private Vec3 _pixelDeltaU;

    private Vec3 _pixelDeltaV;

    private Vec3 _defocusDiskU;

    private Vec3 _defocusDiskV;

    public void Render(IHittable world)
    {
        Initialize();
       
        int width = (int)ImageWidth;
        //buffer for pixels,since we use parallel computing we must take care of order
        Vec3[,] buffer = new Vec3[(int)width, _imageHeight];

        Console.WriteLine("Rendering...");

        var startTime = DateTime.Now;

        Parallel.For(0, _imageHeight, j =>
        {
            for(int i =0; i < width; i++)
            {
                Vec3 pixelColor = new Vec3(0, 0, 0);

                for (int sample = 0; sample < SamplesPerPixel; sample++)
                {
                    Ray ray = GetRay(i, j);

                    pixelColor += RayColor(ray, MaxDepth, world);
                }
                buffer[i, j] = pixelColor;
            }
        });

        Console.Error.WriteLine($"Rendering finished in {(DateTime.Now - startTime).TotalSeconds}\nSaving image...");

        using(var image = new Image<Rgb24>(width, _imageHeight))
        {
            image.ProcessPixelRows(accesor =>
            {
                for (int j = 0; j < _imageHeight; j++)
                {
                    //getting whole row of image pixels
                    var pixelRow = accesor.GetRowSpan(j);
                    for (int i = 0; i < width; i++)
                    {
                        var color = buffer[i, j];

                        double r = color.X / SamplesPerPixel;
                        double g = color.Y / SamplesPerPixel;
                        double b = color.Z / SamplesPerPixel;

                        r = (r > 0) ? Math.Sqrt(r) : 0;
                        g = (g > 0) ? Math.Sqrt(g) : 0;
                        b = (b > 0) ? Math.Sqrt(b) : 0;

                        Interval intensity = new Interval(0.000, 0.999);
                        byte rbyte = (byte)(255.999 * intensity.Clamp(r));
                        byte gbyte = (byte)(255.999 * intensity.Clamp(g));
                        byte bbyte = (byte)(255.999 * intensity.Clamp(b));

                        pixelRow[i] = new Rgb24(rbyte, gbyte, bbyte);
                    }

                }
            });
            image.SaveAsJpeg("render.png");
        }
        Console.WriteLine("Rendering Done! Saved to render.png");
    }


    private Vec3 PixelSampleSquare()
    {
        var px = -0.5 + Utils.RandomDouble();
        var py = -0.5 + Utils.RandomDouble();

        return (px * _pixelDeltaU) + (py * _pixelDeltaV);
    }


    private Ray GetRay(int i, int j)
    {
        var pixelCenter = _pixel00Loc + (i*_pixelDeltaU) + (j * _pixelDeltaV);

        var pixelSample = pixelCenter + PixelSampleSquare();


        var rayOrigin = (DefocusAngle <= 0) ? _center : DefocusDiskSample();

        var rayDirection = pixelSample - rayOrigin;

        return new Ray(rayOrigin, rayDirection);
    }


    private void Initialize()
    {
        _imageHeight = (int)(ImageWidth / AspectRatio);
        _imageHeight = (_imageHeight<1)?1:_imageHeight;

        _center = LookFrom;

        var theta = Utils.DegreesToRadians(Vfov);

        var h = Math.Tan(theta / 2);

        var viewportHeight = 2.0 * h * FocusDist;

        var viewportWidth = viewportHeight * (double)ImageWidth / _imageHeight;

        var w = Vec3.UnitVector(LookFrom - LookAt);

        var u = Vec3.UnitVector(Vec3.Cross(VUp, w));

        var v = Vec3.Cross(w, u);

        var viewportU = viewportWidth * u;
        var viewportV = viewportHeight * -v;

        _pixelDeltaU = viewportU / ImageWidth;
        _pixelDeltaV = viewportV / _imageHeight;

        var viewportUpperLeft = _center - (FocusDist * w) - viewportU / 2 - viewportV / 2;
        
        _pixel00Loc = viewportUpperLeft + 0.5 * (_pixelDeltaU + _pixelDeltaV);

        var defocusRadius = FocusDist * Math.Tan(Utils.DegreesToRadians(DefocusAngle / 2));

        _defocusDiskU = u * defocusRadius;
        _defocusDiskV = v * defocusRadius;
    }

    private Vec3 RayColor(Ray ray, int depth, IHittable world)
    {
        if (depth <= 0)
        {
            return new Color3(0, 0, 0);
        }
        HitRecord rec;
        if (world.Hit(ray, new Interval(0.001, Utils.Infinity), out rec))
        {
            Ray scattered;
            Color3 attenuation;
            if(rec.Material.Scatter(ray,rec, out attenuation, out scattered))
            {
                return attenuation * RayColor(scattered,depth -1, world);
            }
            return new Color3(0, 0, 0);
        }

        Vec3 unitDirection = Vec3.UnitVector(ray.Direction);
        var a = 0.5 * (unitDirection.Y + 1.0);
        return (1.0 - a) * new Vec3(1.0, 1.0, 1.0) + a * new Vec3(0.5, 0.7, 1.0);
    }

    private Vec3 DefocusDiskSample()
    {
        var p = Vec3.RandomInUnitDisk();
        return _center + (p.X * _defocusDiskU) + (p.Y * _defocusDiskV);
    }
}