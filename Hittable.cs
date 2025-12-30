using System;
using System.Numerics;

namespace RayTracer;

public struct HitRecord
{
    public Point3 Point { get; set; }
    public Vector3 Normal { get; set; }
    double T { get; set; }
}


public interface IHittable
{
    bool Hit(Ray ray, double rayTmin, double rayTmax, HitRecord rec);
}