namespace RayTracer.Core;


public static class Utils
{
    public const double Infinity = double.PositiveInfinity;
    public const double Pi = 3.141592654;

    public static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }
    
    public static double RandomDouble()
    {
        // Use the thread-safe Random.Shared to avoid race conditions in parallel rendering
        return Random.Shared.NextDouble();
    }

    public static double RandomDouble(double min, double max)
    {
        return min + (max - min) * RandomDouble();
    }



}