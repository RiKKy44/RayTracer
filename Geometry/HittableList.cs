namespace RayTracer.Geometry;



public class HittableList : IHittable
{
    public List<IHittable> Objects { get; } = new List<IHittable>();

    public void Add(IHittable obj)
    {
        Objects.Add(obj);
    }
    public bool Hit(in Ray ray, Interval rayT, out HitRecord rec)
    {
        HitRecord tempRec;
        bool hitAny = false;
        var closest = rayT.Max;
        
        rec = new HitRecord();

        foreach (var obj in Objects)
        {
            if (obj.Hit(ray,new Interval(rayT.Min,closest), out tempRec))
            {
                hitAny = true;
                closest = tempRec.T;
                rec = tempRec;
            }
        }
        return hitAny;
    }
}
