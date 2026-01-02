using System;

namespace RayTracer;


public class Dielectric : IMaterial
{
    public double Ri {  get; set; }

    public Dielectric(double refractionIndex)
    {
        Ri = refractionIndex;
    }

    public bool Scatter(Ray rIn, HitRecord rec, out Color3 attenuation, out Ray scattered)
    {
        attenuation = new Color3(1.0, 1.0, 1.0);

        double refractionRatio = rec.FrontFace ? (1.0 / Ri) : Ri;

        Vec3 unitDirection = Vec3.UnitVector(rIn.Direction);

        Vec3 refracted = Vec3.Refract(unitDirection, rec.Normal, refractionRatio);

        scattered = new Ray(rec.Point,refracted);

        return true;
    }
}