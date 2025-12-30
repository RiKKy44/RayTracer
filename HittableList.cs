namespace RayTracer;


public class HittableList : IHittable
{
    public List<IHittable> Objects { get; } = new List<IHittable>();

    public void Add(IHittable obj)
    {
        Objects.Add(obj);
    }
    public bool Hit(in Ray ray, double tMin, double tMax, out HitRecord rec)
    {
        HitRecord tempRec;
        bool hitAny = false;
        var closest = tMax;
        
        rec = new HitRecord();

        foreach (var obj in Objects)
        {
            if (obj.Hit(ray, tMin, closest, out tempRec))
            {
                hitAny = true;
                closest = tempRec.T;
                rec = tempRec;
            }
        }
        return hitAny;
    }
}
