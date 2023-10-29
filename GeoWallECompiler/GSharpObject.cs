namespace GeoWallECompiler;
public abstract class GSharpObject
{
    public abstract double ToValueOfTruth();
}
public class GSharpNumber : GSharpObject
{
    public GSharpNumber(double val) => Value = val;
    public override double ToValueOfTruth() => Value == 0 ? 0 : 1;
    public double Value { get; private set; }

    #region Operators Overloads
    public static GSharpNumber operator +(GSharpNumber a) => new(a.Value);
    public static GSharpNumber operator -(GSharpNumber a) => new(-a.Value);
    public static GSharpNumber operator +(GSharpNumber a, GSharpNumber b) => new(a.Value + b.Value);
    public static GSharpNumber operator -(GSharpNumber a, GSharpNumber b) => new(a.Value - b.Value);
    public static GSharpNumber operator *(GSharpNumber a, GSharpNumber b) => new(a.Value * b.Value);
    public static GSharpNumber operator /(GSharpNumber a, GSharpNumber b) => new(a.Value / b.Value);
    public static GSharpNumber operator %(GSharpNumber a, GSharpNumber b) => new(a.Value % b.Value);
    public static GSharpNumber operator <(GSharpNumber a, GSharpNumber b) => a.Value < b.Value? new(1): new(0);
    public static GSharpNumber operator >(GSharpNumber a, GSharpNumber b) => a.Value > b.Value ? new(1) : new(0);
    public static GSharpNumber operator <=(GSharpNumber a, GSharpNumber b) => a.Value <= b.Value ? new(1) : new(0);
    public static GSharpNumber operator >=(GSharpNumber a, GSharpNumber b) => a.Value >= b.Value ? new(1) : new(0);
    public static GSharpNumber operator ==(GSharpNumber a, GSharpNumber b) => a.Value == b.Value ? new(1) : new(0);
    public static GSharpNumber operator !=(GSharpNumber a, GSharpNumber b) => a.Value != b.Value ? new(1) : new(0);
    #endregion
}
public class GSharpString : GSharpObject
{
    public GSharpString(string val) => Value = val;
    public override double ToValueOfTruth() => Value == "" ? 0 : 1;
    public string Value { get; private set; }
}
public class GSharpPoint : GSharpObject, IDrawable
{
    public GSharpPoint(GSharpNumber? x, GSharpNumber? y)
    {
        if (x is not null && y is not null)
            Coordinates = (x, y);
        else
            Coordinates = null;
    }
    public override double ToValueOfTruth() => Coordinates is null? 0 : 1;
    public void Draw(IDrawer drawer) => drawer.DrawPoint(this);
    public (GSharpNumber X, GSharpNumber Y)? Coordinates { get; private set; }
}
public class GSharpLine : GSharpObject, IDrawable
{
    public GSharpLine(GSharpPoint? point1, GSharpPoint? point2)
    {
        Point1 = point1;
        Point2 = point2;
    }
    public GSharpPoint? Point1 { get; private set; }
    public GSharpPoint? Point2 { get; private set; }
    public void Draw(IDrawer drawer) => throw new NotImplementedException();
    public override double ToValueOfTruth() => Point1 is null || Point2 is null ? 0 : 1;
}
