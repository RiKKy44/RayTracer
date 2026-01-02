namespace RayTracer;


public class Metal : IMaterial
{
    public Color3 Albedo { get; set; }

    public Metal(Color3 albedo)
    {
        Albedo = albedo;
    }

    public bool Scatter(Ray ray, HitRecord rec, out Color3 attenuation, out Ray scattered)
    {
        Vec3 reflected = Vec3.Reflect(Vec3.UnitVector(ray.Direction), rec.Normal);

        scattered = new Ray(rec.Point, reflected);

        attenuation = Albedo;

        return true;
    }
}