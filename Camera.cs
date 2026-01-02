using System;
using System.Threading.Tasks;
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
       
        //buffer for pixels,since we use parallel computing we must take care of order
        Vec3[,] buffer = new Vec3[(int)ImageWidth,_imageHeight];

        Console.Error.WriteLine("Rendering...");

        var startTime = DateTime.Now;

        Parallel.For(0, _imageHeight, j =>
        {
            for(int i =0; i < ImageWidth; i++)
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

        Console.Error.WriteLine($"Rendering finished in {(DateTime.Now - startTime).TotalSeconds}\nWriting to file...");

        using(StreamWriter writer = new StreamWriter("image.ppm"))
        {
            writer.WriteLine("P3");
            writer.WriteLine($"{ImageWidth} {_imageHeight}");
            writer.WriteLine("255");
            

            for(int j = 0; j < _imageHeight; j++)
            {
                for(int i = 0; i < ImageWidth; i++)
                {
                    Color.WriteColor(writer, buffer[i,j], SamplesPerPixel);
                }
            }
            Console.Error.Write("\nDone");
        }
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
        var p = RandomInUnitDisk();
        return _center + (p.X * _defocusDiskU) + (p.Y * _defocusDiskV);
    }
}