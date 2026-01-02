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

        double cosTheta = Math.Min(Vec3.Dot(-unitDirection, rec.Normal), 1.0);

        double sinTheta = Math.Sqrt(1.0 - cosTheta * cosTheta);

        bool cannotRefract = refractionRatio * sinTheta > 1.0;

        Vec3 direction;

        if (cannotRefract || Reflectance(cosTheta, refractionRatio) > Utils.RandomDouble())
        {
            direction = Vec3.Reflect(unitDirection, rec.Normal);
        }
        else
        {
            direction = Vec3.Refract(unitDirection, rec.Normal, refractionRatio);
        }
           
        scattered = new Ray(rec.Point,direction);

        return true;
    }

    private static double Reflectance(double cosine, double refIdx)
    {
        var r0 = (1 - refIdx) / (1 + refIdx);
        r0 = r0 * r0;
        return r0 + (1 - r0) * Math.Pow((1 - cosine), 5);
    }
}