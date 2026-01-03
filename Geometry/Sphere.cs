namespace RayTracer.Geometry;

public class Sphere: IHittable
{
    public double Radius { get; set; }
    public Point3 Center { get; set; }
    public IMaterial Material { get; set; }

    public Sphere(Point3 center, double radius,IMaterial material)
    {
        Center = center;
        Radius = (radius < 0) ? 0 : radius;
        Material = material;
    }

    public bool Hit(in Ray ray, Interval rayT, out HitRecord rec)
    {
        Vec3 oc = Center - ray.Origin;
        var a = ray.Direction.LengthSquared();
        var h = Vec3.Dot(ray.Direction, oc);
        var c = oc.LengthSquared() - Radius * Radius;

        var discriminant = h * h - a * c;
        if (discriminant < 0)
        {
            rec = default;
            return false;
        }
            
        var sqrtd = Math.Sqrt(discriminant);
        
        var root = (h - sqrtd) / a;
        if (!rayT.Surrounds(root))
        {
            root = (h + sqrtd) / a;
            if (!rayT.Surrounds(root))
            {
                rec = default;
                return false;
            }
        }

        rec = new HitRecord();
        rec.T = root;
        rec.Point = ray.At(rec.T);
        Vec3 outwardNormal = (rec.Point - Center) / Radius;
        rec.SetFaceNormal(ray, outwardNormal);
        rec.Material = Material;
        return true;
    }
    
}