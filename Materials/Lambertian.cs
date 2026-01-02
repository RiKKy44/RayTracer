namespace RayTracer;


public class Lambertian : IMaterial
{
    public Color3 Albedo {  get; set; }

    public Lambertian(Color3 albedo)
    {
        Albedo = albedo;
    }

    public bool Scatter(Ray ray, HitRecord rec, out Color3 attenuation, out Ray scattered)
    {
        var scatterDirection = rec.Normal + Vec3.RandomUnitVector();

        if (scatterDirection.NearZero()) scatterDirection = rec.Normal;

        scattered = new Ray(rec.Point,scatterDirection);
        attenuation = Albedo;
        return true;
    }
    
}