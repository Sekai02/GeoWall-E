namespace GeoWallECompiler.Objects;
public class Vector
{
    public Vector(double x, double y)
    {
        X = x;
        Y = y;
        Norm = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
    }

    public double X { get; }
    public double Y { get; }
    public double Norm { get; }
    public Vector GetNormalized()
    {
        double x = X / Norm;
        double y = Y / Norm;
        return new Vector(x, y);
    }
    public static GSPoint operator +(GSPoint p, Vector v)
    {
        double x = p.Coordinates.X + v.X;
        double y = p.Coordinates.Y + v.Y;
        return new((GSNumber)x, (GSNumber)y);
    }
    public static Vector operator *(double alpha, Vector v)
    {
        double x = v.X * alpha;
        double y = v.Y * alpha;
        return new Vector(x, y);
    }
}
