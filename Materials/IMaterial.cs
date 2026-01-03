namespace RayTracer.Materials;


public interface IMaterial
{
    public bool Scatter(Ray ray, HitRecord rec, out Color3 attenuation, out Ray scattered);
}