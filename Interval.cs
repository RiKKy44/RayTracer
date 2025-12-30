namespace RayTracer;

public struct Interval
{
    public double Min { get; }
    public double Max { get; }


    public Interval(double min, double max)
    {
        Min = min;
        Max = max;
    }
    
    public Interval(): this(double.PositiveInfinity, double.NegativeInfinity) { }
    public bool Contains(double x) => x >= Min && x < Max;

    public bool Surrounds(double x) => x > Min && x < Max;
    
    public static readonly Interval Empty = new Interval(double.PositiveInfinity, double.NegativeInfinity);
    
    public static readonly Interval Universe = new Interval(double.NegativeInfinity, double.PositiveInfinity);

}