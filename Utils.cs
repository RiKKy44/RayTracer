global using Color3 = RayTracer.Vec3;
global using Point3 = RayTracer.Vec3;
using System;
namespace RayTracer;

public static class Utils
{
    public const double Infinity = double.PositiveInfinity;
    public const double Pi = 3.141592654;

    public static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
    
}