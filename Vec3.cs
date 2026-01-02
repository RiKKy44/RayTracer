
namespace RayTracer;

//readonly to prevent compiler from making defensive copies when passing vec3 by reference
public readonly struct Vec3
{
    public double X { get; }
    public double Y { get; }
    public double Z { get; }

    public Vec3(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vec3()
    {
        X = 0;
        Y = 0;
        Z = 0;
    }

    public double this[int index] => index switch
    {
        0 => X,
        1 => Y,
        2 => Z,
        _ => throw new IndexOutOfRangeException()
    };

    public static Vec3 operator -(in Vec3 v)=> new Vec3(-v.X, -v.Y, -v.Z);
    
    public static Vec3 operator +(in Vec3 v1, in Vec3 v2) => new Vec3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
    
    public static Vec3 operator -(in Vec3 v1, in Vec3 v2)=> new Vec3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
    //scalar multiplication
    public static Vec3 operator *(in Vec3 v, double s) => new Vec3(v.X * s, v.Y * s, v.Z * s);
    
    //commutative scalar multiplication
    public static Vec3 operator *(double s, in Vec3 v) => v * s;
    
    public static Vec3 operator /(in Vec3 v, double s) => new Vec3(v.X / s, v.Y / s, v.Z / s);
    //hadamard product
    public static Vec3 operator *(in Vec3 v1, in Vec3 v2) => new Vec3(v1.X*v2.X, v1.Y*v2.Y, v1.Z*v2.Z);
    public double Length() => Math.Sqrt(X * X + Y * Y + Z * Z);
    
    public double LengthSquared() => X * X + Y * Y + Z * Z;
    
    public static double Dot(in Vec3 v1, in Vec3 v2) => v1.X*v2.X + v1.Y*v2.Y + v1.Z*v2.Z;
    
    public static Vec3 Cross(in Vec3 v1, in Vec3 v2) =>
        new(v1.Y * v2.Z - v1.Z * v2.Y,
            v1.Z * v2.X - v1.X * v2.Z,
            v1.X * v2.Y - v1.Y * v2.X);

    public static Vec3 UnitVector(in Vec3 v) => v / v.Length();

    public override string ToString() => $"{X} {Y} {Z}";    


    public static Vec3 Random()
    {
        return new Vec3(Utils.RandomDouble(), Utils.RandomDouble(), Utils.RandomDouble());
    }

    public static Vec3 Random(double min, double max) =>
        new Vec3(Utils.RandomDouble(min,max),Utils.RandomDouble(min,max),Utils.RandomDouble(min,max));

    public static Vec3 RandomUnitVector()
    {
        while (true)
        {
            var p = Vec3.Random(-1, 1);
            var lensq = p.LengthSquared();
            if (lensq <= 1)
            {
                return p / Math.Sqrt(lensq);
            }

        }
    }
    public static Vec3 RandomOnHemisphere(Vec3 normal)
    {
        Vec3 onUnitSphere = RandomUnitVector();
        if (Vec3.Dot(onUnitSphere, normal) > 0.0)
        {
            return onUnitSphere;
        }
        else
        {
            return -onUnitSphere;
        }
    }

    public bool NearZero()
    {
        var s = 1e-8;
        return (Math.Abs(X) < s) && (Math.Abs(Y) < s) && (Math.Abs(Z) < s);
    }

    public static Vec3 Reflect(Vec3 v, Vec3 n)
    {
        return v - 2 * Dot(v, n) * n;
    }

    public static Vec3 Refract(Vec3 uv, Vec3 n, double etaiOverEtat)
    {
        var cosTheta = Math.Min(Vec3.Dot(-uv, n), 1.0);

        Vec3 rOutPerp = etaiOverEtat * (uv + cosTheta * n);

        var rOutParallel = -Math.Sqrt(Math.Abs(1.0 - rOutPerp.LengthSquared())) * n;
        
        return rOutPerp + rOutParallel;
    }
}
