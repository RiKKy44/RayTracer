
namespace RayTracer;

public readonly struct Ray
{
    public Point3 Origin { get; }
    public Vec3 Direction { get; }
    
    public Ray(in Point3 origin, in Vec3 direction)
    { 
        Origin = origin;
        Direction = direction;
    }

    public Point3 At(double time)
    {
        return Origin + Direction * time;
    }
    
    
}