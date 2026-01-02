namespace RayTracer;


public Interface IMaterial
{
    public bool Scatter(Ray ray, HitRecord rec, out Color3 attenuation, out Ray scattered);
}