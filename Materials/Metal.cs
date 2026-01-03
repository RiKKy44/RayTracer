namespace RayTracer.Materials;


public class Metal : IMaterial
{
    public Color3 Albedo { get; set; }

    private double _fuzz;

    public Metal(Color3 albedo, double fuzz)
    {
        Albedo = albedo;
        _fuzz = (fuzz < 1) ? fuzz : 1;

    }

    public bool Scatter(Ray ray, HitRecord rec, out Color3 attenuation, out Ray scattered)
    {
        Vec3 reflected = Vec3.Reflect(Vec3.UnitVector(ray.Direction), rec.Normal);

        scattered = new Ray(rec.Point, reflected + _fuzz*Vec3.RandomUnitVector());

        attenuation = Albedo;

        return (Vec3.Dot(scattered.Direction, rec.Normal) > 0);
    }
}