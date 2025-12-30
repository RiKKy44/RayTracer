using System;

namespace RayTracer;

public struct HitRecord
{
    public Point3 Point { get; set; }
    public Vec3 Normal { get; set; }
    public double T { get; set; }
    
    public bool FrontFace {get; set;}

    //sets the hit record normal vector
    public void SetFaceNormal(Ray ray, Vec3 outwardNormal)
    {
        FrontFace = Vec3.Dot(ray.Direction, outwardNormal) < 0;
        Normal = FrontFace ? outwardNormal : -outwardNormal;
    }
}


public interface IHittable
{
    bool Hit(in Ray ray, Interval rayT, out HitRecord rec);
}