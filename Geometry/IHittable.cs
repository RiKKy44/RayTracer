using System;

namespace RayTracer;

public interface IHittable
{
    bool Hit(in Ray ray, Interval rayT, out HitRecord rec);
}